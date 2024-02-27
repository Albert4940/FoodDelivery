using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace FoodDeliveryWebApp.Models
{
    public class SessionExpireAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            /* var controllerName = filterContext.Controller.GetType().Name;
             var actionNane = filterContext.ActionDescriptor.ActionName;*/

            /*var controllerName = filterContext.RouteData.Values["controller"]?.ToString();
            var actionName = filterContext.RouteData.Values["action"]?.ToString();
           
            //make algorithm to get any params dynamicaly
            var firstItem = filterContext.ActionArguments["OrderId"];
             
            var redirect = $"/{controllerName}/{actionName}/?OrderId={firstItem}";*/

            var session = filterContext.HttpContext.Session;
            if(session != null && string.IsNullOrEmpty(session.GetString("JWToken"))) 
            {
                //filterContext.Result = new RedirectResult($"/User/Index?redirect={redirect}");
                filterContext.Result = new RedirectResult($"/User/Index");
            }
        }
    }
}
