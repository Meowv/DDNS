using DDNS.Entity.Users;
using DDNS.Entity.Verify;
using DDNS.Provider.Users;
using DDNS.Provider.Verify;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System.Threading.Tasks;

namespace DDNS.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly VerifyProvider _verifyProvider;
        private readonly UsersProvider _usersProvider;
        private readonly IStringLocalizer<AccountController> _localizer;

        public AccountController(VerifyProvider verifyProvider, UsersProvider usersProvider, IStringLocalizer<AccountController> localizer)
        {
            _verifyProvider = verifyProvider;
            _usersProvider = usersProvider;
            _localizer = localizer;
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public IActionResult Login()
        {
            return View();
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        public IActionResult Register()
        {
            return View();
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <returns></returns>
        public IActionResult Forget()
        {
            return View();
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        public IActionResult Reset(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var verify = _verifyProvider.GetVerifyInfo(token, VerifyTypeEnum.Password);
            if (verify != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }

        /// <summary>
        /// 激活验证
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Verify(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }

            var verify = _verifyProvider.GetVerifyInfo(token, VerifyTypeEnum.Register);
            if (verify != null)
            {
                var user = await _usersProvider.GetUserInfo(verify.UserId);
                user.Status = (int)UserStatusEnum.Normal;
                await _usersProvider.UpdateUser(user);

                await _verifyProvider.VerifySuccess(token);

                ViewData["Message"] = _localizer["verifyed"];
            }
            else
            {
                ViewData["Message"] = _localizer["expired"];
            }

            return View();
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <returns></returns>
        public IActionResult Password()
        {
            return View();
        }

        /// <summary>
        /// 退出
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction(nameof(HomeController.Index), "Home");
        }
    }
}