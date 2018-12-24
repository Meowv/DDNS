using DDNS.Provider.LoginLog;
using DDNS.Provider.Users;
using DDNS.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DDNS.Web.Controllers
{
    [Authorize]
    public class ConsoleController : Controller
    {
        private readonly UsersProvider _usersProvider;
        private readonly LoginLogProvider _loginLogProvider;

        public ConsoleController(UsersProvider usersProvider, LoginLogProvider loginLogProvider)
        {
            _usersProvider = usersProvider;
            _loginLogProvider = loginLogProvider;
        }

        /// <summary>
        /// 框架页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _usersProvider.GetUserInfo(Convert.ToInt32(userId));

            return View(user);
        }

        /// <summary>
        /// 默认显示页面
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Home()
        {
            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _usersProvider.GetUserInfo(Convert.ToInt32(userId));

            var log = await _loginLogProvider.GetLastLoginLog(Convert.ToInt32(userId));

            var info = new UserInfoViewModel
            {
                UserName = user.UserName,
                Email = user.Email,
                RegisterTime = user.RegisterTime,
                LastLoginTime = log.LoginTime,
                LastLoginIP = log.LoginIp
            };

            return View(info);
        }
    }
}