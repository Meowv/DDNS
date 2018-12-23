using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DDNS.Web.Controllers
{
    [Authorize]
    public class ConsoleController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}