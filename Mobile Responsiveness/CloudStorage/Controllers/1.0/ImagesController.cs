using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Linq;
using System.Net;
using Asp.Versioning;
using CloudStorage.Services;
using CloudStorage.Entities.V0U0;
using CloudStorage.Models;

namespace CloudStorage.Controllers.V0U0
{
    [ApiController]
    [Route("api/[controller]")]
    [ApiVersion(1.0)]
    [ApiVersion("2023-11-01")]
    public class ImagesController : Controller
    {
        private readonly IImageTableStorage imageTableStorage;
        private readonly IUserNameProvider userNameProvider;

        public ImagesController(IImageTableStorage imageTableStorage, IUserNameProvider userNameProvider)
        {
            this.userNameProvider = userNameProvider;
            this.imageTableStorage = imageTableStorage;
        }

        [HttpGet]
        public IAsyncEnumerable<ImageEntity> GetAsync()
        {
            return imageTableStorage.GetAllImagesAsync().Select(image => new ImageEntity(image));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetAsync(string id)
        {
            var imageModel = await this.imageTableStorage.GetAsync(id);

            // check to make sure imageModel is not null
            // if it is null (i.e. it doesn't exist), return not found

            if (imageModel == null)
            {
                return NotFound();
            }

            // set Cache-Control header here, it is in seconds; cached for seven hours
            Response.Headers["Cache-Control"] = "max-age=25200";

            // return actual download url in the Location header
            Response.Headers["Location"] = imageTableStorage.GetDownloadUrl(imageModel);

            // return response status code 302, found
            return StatusCode((int)HttpStatusCode.Found);
        }

        [HttpPost]
        public async Task<IActionResult> PostAsync([FromBody] ImageEntity imageEntity)
        {
            // Convert the image entity into an image model, then add it into the database.
            // set the username property on the image model before it is added.

            ImageModel imageModel = imageEntity.ToModel();

            imageModel.UserName = this.userNameProvider.UserName;

            var tableImage = await imageTableStorage.AddOrUpdateAsync(imageModel);

            var sasUploadURL = imageTableStorage.GetUploadUrl(tableImage.Id.ToString());

            ImageEntity newImageEntity = new ImageEntity();

            //Set new image entity values
            newImageEntity.UploadUrl = sasUploadURL;
            newImageEntity.Id = tableImage.Id;
            newImageEntity.Name = tableImage.Name;


            // Returns a new image entity to the client. Sets the uploadUrl first so user can start the image upload.

            return Ok(newImageEntity);
        }

        [HttpPut("{id}/uploadComplete")]
        public async Task<IActionResult> UploadCompleteAsync(string id)
        {
            // Get the image model from the database by its id.
            ImageModel imageModel = await imageTableStorage.GetAsync(id);

            // check to make sure image model is not null
            // if it is null (i.e. it doesn't exist), return a NotFound status code
            if (imageModel == null)
            {
                return NotFound();
            }
            
            // Set UploadComplete to true on the imageModel and then save it.
            imageModel.UploadComplete = true;
            // Update image table entry with new model
            await imageTableStorage.AddOrUpdateAsync(imageModel);

            // Convert the image model into an ImageEntity and return it as JSON.
            ImageEntity imageEntity = new ImageEntity(imageModel);

            return Ok(imageEntity);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(string id)
        {
            await this.imageTableStorage.DeleteAsync(id);
            return StatusCode((int)HttpStatusCode.NoContent);
        }

        [HttpDelete]
        public async Task<IActionResult> PurgeAsync()
        {
            await this.imageTableStorage.PurgeAsync();
            return StatusCode((int)HttpStatusCode.NoContent);
        }
    }
}