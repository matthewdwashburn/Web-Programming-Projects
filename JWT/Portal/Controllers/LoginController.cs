using Microsoft.AspNetCore.Mvc;
using Portal.Models;
using Portal.Services;
using System.Security.Claims;

namespace Portal.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : Controller
    {
        private readonly SecurityProvider securityProvider;

        private readonly UserDatabase database;

        public LoginController(SecurityProvider securityProvider, UserDatabase database)
        {
            this.securityProvider = securityProvider;
            this.database = database;
        }

        //User attempts to login with username and password
        [HttpPost]
        public IActionResult Post([FromBody] UserModel user)
        {
            //Check if no username sent
            if (user.UserName == null)
            {
                return BadRequest("Please input a username.");
            }

            //Check if username exists
            if(database.GetUser(user.UserName) == null)
            {
                return NotFound("Username not found.");
            }

            //Check if username matches password
            UserModel storedUser = database.GetUser(user.UserName);
            if(storedUser.Password != user.Password)
            {
                return Unauthorized("Password is incorrect for this username.");
            }

            //Username exists and password matches, return authorization token for parameter username
            var claims = new List<Claim>() { new Claim("username", user.UserName) };
            var token = securityProvider.GetToken(claims);

            return new ContentResult()
            {
                Content = token
            };
        }
    }
}