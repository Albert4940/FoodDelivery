using FoodDeliveryAPI.Data;
using FoodDeliveryAPI.Models;
using FoodDeliveryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BC = BCrypt.Net.BCrypt;

namespace FoodDeliveryAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        public UserController(FoodDeliveryContext context, IConfiguration configuration)
        {
            _configuration = configuration;
            UserService.InitializeContext(context);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(await UserService.GetAll());
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
                bool userExists = await UserService.CheckIfUserExists(userRegister);

                if (userExists)
                    return Conflict("User Already exist !");

                user.Password = BC.HashPassword(user.Password, 12);

                await UserService.Add(userRegister);
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
            var user = await UserService.Authenticate(userLogin);

            if (user == null || !BC.Verify(userLogin.Password, user.Password))
            {
                return NotFound("User not found");
            }
            else
            {
                var token = TokenService.Generate(user, _configuration);
                return Ok(token);
            }
            /*if (user != null )
            {
                var token = TokenService.Generate(user, _configuration);
                return Ok(token);
            }

            return NotFound("User not found");*/
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
                    Id = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.PrimarySid)?.Value,
                    UserName = userClaims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value,
                };
            }
            return null;
        }
    }
}
