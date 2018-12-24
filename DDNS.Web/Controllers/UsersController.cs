using Microsoft.AspNetCore.Mvc;

namespace DDNS.Web.Controllers
{
    public class UsersController : Controller
    {
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
        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }

        /// <summary>
        /// 用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public IActionResult Detail(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            return View();
        }
    }
}