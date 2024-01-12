using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly FoodDeliveryContext _context;
        private readonly IConfiguration _configuration;
        public UserController(FoodDeliveryContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Register(User userRegister)
        {


            // var user = await UserService.Authenticate(userLogin, _context);
            var user = userRegister;
            if ((user.UserName != null) && user.Password != null)
            {
                //check if empty
                //var token = TokenService.Generate(user, _configuration);
                bool userExists = await UserService.CheckIfUserExists(userRegister, _context);

                if (userExists)
                    return Conflict("User Already exist !");

                await UserService.Add(userRegister, _context);
                return Ok("User Created");
            }
            else
            {
                return BadRequest();
            }

            return BadRequest();
        }
        [AllowAnonymous]
        [HttpPost("auth")]
        public async Task<IActionResult> Auth(User userLogin)
        {
            var user = await UserService.Authenticate(userLogin, _context);

            if (user != null)
            {
                var token = TokenService.Generate(user, _configuration);
                return Ok(token);
            }

            return NotFound("User not found");
        }

        //user/auth
        [HttpGet("admin")]
        [Authorize]
        public async Task<IActionResult> AdminsEndPoint()
        {
            var currentUser = GetCurrent();
            var res = $"Hi {currentUser.UserName}";
            // return new User { UserName = currentUser.UserName};
            return Ok(currentUser);
        }

        private User GetCurrent()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var userClaims = identity.Claims;

                return new User
                {
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                };
            }
            return null;
        }
    }
}
