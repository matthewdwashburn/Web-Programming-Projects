using CloudStorage.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CloudStorage.Entities.V0U1
{
    public class ImageEntity
    {
        public ImageEntity()
        {

        }

        public ImageEntity(ImageModel imageModel)
        {
            this.Name = imageModel.Name;

            this.Description = imageModel.Description;

            this.Id = imageModel.Id;
        }

        [MinLength(3)]

        public string Name { get; set; }

        [Required]
        [MinLength(5)]
        public string Description { get; set; }

        public string Id { get; internal set; }

        public string UploadUrl { get; internal set; }



        /// <summary>
        /// Convert the ImageEntity to a model that can be saved to the database.
        /// Note that this method automatically sets the a new Id for the image.
        /// </summary>
        /// <returns></returns>
        public ImageModel ToModel()
        {
            return new ImageModel()
            {
                Name = this.Name,
                Description = this.Description,
                Id = Guid.NewGuid().ToString()
            };
        }
    }
}