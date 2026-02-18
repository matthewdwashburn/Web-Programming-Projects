using Mario.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Threading.Tasks;

namespace Mario.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarioController : Controller
    {
        private readonly IMarioServices marioService;
        public MarioController(IMarioServices marioService)
        {
            this.marioService = marioService;
        }
        [HttpGet("{move}")]
        public async Task<IActionResult> GetAsync(string move)
        {

            if(move != "walk" && move != "run" && move != "wait" && move != "jump")
            {
                return new JsonResult(new { message = "Invalid value for move parameter: Move value must be \"walk\", \"run\", \"wait\", or \"jump\"." });
            }
            try
            {
                return Json(await marioService.GetMarioLevelStatusAsync(move));
            }
            catch (Exception)
            {
                return new JsonResult(new { message = "Mario died." })
                {
                    StatusCode = (int)HttpStatusCode.OK
                };
            }
        }


    }
}
