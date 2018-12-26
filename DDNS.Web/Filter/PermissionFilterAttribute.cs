using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Security.Claims;

namespace DDNS.Web.Filter
{
    /// <summary>
    /// 权限验证
    /// </summary>
    public class PermissionFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var isAdmin = context.HttpContext.User.FindFirst(u => u.Type == ClaimTypes.Role).Value;
            if (Convert.ToInt32(isAdmin) != 1)
            {
                context.Result = new ContentResult()
                {
                    Content = "UnAuthorized"
                };
            }
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
        }
    }
}