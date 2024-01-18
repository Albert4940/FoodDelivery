using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryWebApp.Models
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
    public class SessionAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            //var userId = context.HttpContext.Session.GetString("UserId");
            var token = context.HttpContext.Session.GetString("JWToken");
            if (token == null)
            {
                context.Result = new RedirectToActionResult("Index", "User", null);
            }
        }
    }

}
