using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DDNS.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //Response.Cookies.Append(
            //    CookieRequestCultureProvider.DefaultCookieName,
            //    CookieRequestCultureProvider.MakeCookieValue(new RequestCulture("en-US")),
            //    new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            //);

            return View();
        }
    }
}