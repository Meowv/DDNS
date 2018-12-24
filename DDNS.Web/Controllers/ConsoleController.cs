using DDNS.Provider.Users;
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
        public ConsoleController(UsersProvider usersProvider)
        {
            _usersProvider = usersProvider;
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
        public IActionResult Home()
        {
            return View();
        }
    }
}