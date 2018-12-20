using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace DDNS.Web.API
{
    [Route("api")]
    [ApiController]
    public class CommonController : ControllerBase
    {
        /// <summary>
        /// 设置网站语言
        /// </summary>
        /// <param name="culture"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("culture")]
        public ActionResult<int> ChangeCulture(string culture)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return Ok(1);
        }
    }
}