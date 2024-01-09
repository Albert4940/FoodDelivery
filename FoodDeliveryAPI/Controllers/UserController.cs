using FoodDeliveryAPI.Models;
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
