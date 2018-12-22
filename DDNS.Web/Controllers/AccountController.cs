using DDNS.Entity.Users;
using DDNS.Provider.Users;
using DDNS.Provider.Verify;
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
        public IActionResult Reset()
        {
            return View();
        }

        /// <summary>
        /// 激活验证
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Verify(string token)
        {
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Index", "Home");
            }

            var verify = _verifyProvider.GetVerifyInfo(token);
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
    }
}