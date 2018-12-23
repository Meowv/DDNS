using DDNS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;

namespace DDNS.Web.API.Common
{
    [Route("api")]
    [ApiController]
    public class CommonApiController : ControllerBase
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

        /// <summary>
        /// 验证码
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("verifycode")]
        public IActionResult VerifyCode()
        {
            MemoryStream ms = VerifyCodeUtil.GenerateCode(out string code);
            HttpContext.Session.SetString("verify_code", code);
            Response.Body.Dispose();
            return File(ms.ToArray(), @"image/png");
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("check_verifycode")]
        public IActionResult CheckVerifyCode(string code)
        {
            if (!string.IsNullOrEmpty(code))
            {
                var sessionCode = HttpContext.Session.GetString("verify_code");
                return Ok(string.Equals(sessionCode.Trim(), code.Trim(), StringComparison.CurrentCultureIgnoreCase));
            }
            return Ok(false);
        }
    }
}