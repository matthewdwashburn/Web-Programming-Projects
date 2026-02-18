using Gargoyles.Entities;
using Gargoyles.Models;
using Gargoyles.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using System.Net;
namespace Gargoyles.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GargoylesController : Controller
    {
        private readonly GargoylesDatabase database;
        public GargoylesController(GargoylesDatabase database)
        {
            this.database = database;

        }

        // GET: api/gargoyles - Returns all gargoyles
        [HttpGet]
        public IActionResult GetAll()
        {
            return Json(this.database.GetAll());
        }

        // GET: api/gargoyles/{name} - Returns a specific gargoyle by name
        [HttpGet("{name}")]
        public IActionResult Get(string name)
        {
            var gargoyleModel = database.Get(name);

            if (gargoyleModel == null)
            {
                return NotFound();
            }

            string currentETag = gargoyleModel.ETag();
            Response.Headers["ETag"] = currentETag;

            // Check for If-None-Match header - return 304 if ETag matches
            if (Request.Headers.TryGetValue("If-None-Match", out StringValues ifNoneMatch))
            {
                if (ifNoneMatch == currentETag || ifNoneMatch == "*")
                {
                    return StatusCode(304); // Not Modified - return empty body
                }
            }


            return Json(new GargoyleEntity(gargoyleModel));
        }

        // POST: api/gargoyles - Creates a new gargoyle
        [HttpPost]
        public IActionResult Post([FromBody] GargoyleEntity gargoyleEntity)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var verifyNotDuplicate = this.database.Get(gargoyleEntity.Name);

            // Create new gargoyle if name doesn't exist
            if (verifyNotDuplicate == null)
            {
                GargoyleModel gargoyleModel = gargoyleEntity.ToModel();
                this.database.AddOrReplace(gargoyleModel);
                return Json(new GargoyleEntity(gargoyleModel));
            }
            // Return 409 Conflict if gargoyle name already exists
            return Conflict();
        }

        // PUT: api/gargoyles/{name} - Replaces an entire gargoyle
        [HttpPut("{name}")]
        public IActionResult Put(string name, [FromBody] GargoyleEntity gargoyleEntity)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure URL name matches body name
            if (name != gargoyleEntity.Name)
            {
                return BadRequest();
            }

            var gargoyleModel = this.database.Get(name);

            // Require if-match header with valid ETag for updates
            if (gargoyleModel != null)
            {

                if (Request.Headers.TryGetValue("if-match", out StringValues ifMatch))
                {
                    if (ifMatch != gargoyleModel.ETag() && ifMatch != "*")
                    {
                        return StatusCode((int)HttpStatusCode.PreconditionFailed);
                    }
                }
            }
            var newGargoyleModel = gargoyleEntity.ToModel();
            this.database.AddOrReplace(newGargoyleModel);
            return Json(new GargoyleEntity(newGargoyleModel));

        }

        // PATCH: api/gargoyles/{name} - Partially updates a gargoyle
        [HttpPatch("{name}")]
        public IActionResult Patch(string name, [FromBody] GargoyleEntity gargoyleEntity)
        {
            // Validate input
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var gargoyleModel = this.database.Get(name);

            if (gargoyleModel == null)
            {
                return NotFound();
            }

            // Require if-match header with valid ETag
            if (Request.Headers.TryGetValue("if-match", out StringValues ifMatch))
            {
                if (ifMatch != gargoyleModel.ETag() && ifMatch != "*")
                {
                    return StatusCode((int)HttpStatusCode.PreconditionFailed);
                }
            }

            // Only update fields that were provided (non-null)
            if (gargoyleEntity.Name != null)
            {
                gargoyleModel.Name = gargoyleEntity.Name;
            }
            if (gargoyleEntity.Color != null)
            {
                gargoyleModel.Color = gargoyleEntity.Color;
            }
            if (gargoyleEntity.Size != null)
            {
                gargoyleModel.Size = gargoyleEntity.Size;
            }
            if (gargoyleEntity.Gender != null)
            {
                gargoyleModel.Gender = gargoyleEntity.Gender;
            }
            this.database.AddOrReplace(gargoyleModel);
            return Json(new GargoyleEntity(gargoyleModel));

        }
    }
}