using Microsoft.AspNetCore.Mvc;

namespace DDNS.Web.Controllers
{
    public class ConsoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}