using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demand.Core.Attribute
{
    public class UserTokenAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var userIdClaim = context.HttpContext.User.FindFirst("UserId");

            if (userIdClaim == null || string.IsNullOrEmpty(userIdClaim.Value))
            {
                context.Result = new RedirectToActionResult("Login", "Home", new { returnUrl = context.HttpContext.Request.Path.Value + context.HttpContext.Request.QueryString.Value });

                //context.Result = new RedirectToRouteResult(new Microsoft.AspNetCore.Routing.RouteValueDictionary(new { controller = "Home", action = "Login" }));
            }

            base.OnActionExecuting(context);
        }
    }
}
