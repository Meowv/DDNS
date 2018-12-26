using DDNS.Provider.Users;
using DDNS.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DDNS.Web.Controllers
{
    [Authorize]
    [PermissionFilter]
    public class UsersController : Controller
    {
        private readonly UsersProvider _usersProvider;

        public UsersController(UsersProvider usersProvider)
        {
            _usersProvider = usersProvider;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _usersProvider.GetUserInfo(id ?? 0);

            return View(user);
        }
    }
}