using Microsoft.AspNetCore.Mvc;

namespace DDNS.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Register()
        {
            return View();
        }

        public IActionResult Forget()
        {
            return View();
        }

        public IActionResult Reset()
        {
            return View();
        }
    }
}