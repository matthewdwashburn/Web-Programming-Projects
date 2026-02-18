using Microsoft.AspNetCore.Mvc;
using Portal.Filters;
using Portal.Services;
using System.Security.Cryptography;

namespace Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [ServiceFilter(typeof(AuthorizationFilter))]
    public class GladosController : Controller
    {
        private readonly IGladosService gladosService;

        public GladosController(IGladosService gladosService) { 
            this.gladosService = gladosService;
        }

        [HttpGet]
        public IActionResult Get()
        {

            //Return random glados quote with glados helper service
            return Ok(gladosService.GetGladosQuote());
        }
    }
}