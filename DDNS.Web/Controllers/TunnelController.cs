using DDNS.Entity.AppSettings;
using DDNS.Provider.Tunnel;
using DDNS.Provider.Users;
using DDNS.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace DDNS.Web.Controllers
{
    [Authorize]
    public class TunnelController : Controller
    {
        private readonly UsersProvider _usersProvider;
        private readonly TunnelProvider _tunnelProvider;
        private readonly TunnelConfig _tunnelConfig;

        public TunnelController(UsersProvider usersProvider, TunnelProvider tunnelProvider, IOptions<TunnelConfig> config)
        {
            _usersProvider = usersProvider;
            _tunnelProvider = tunnelProvider;
            _tunnelConfig = config.Value;
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// 开通隧道
        /// </summary>
        /// <returns></returns>
        public IActionResult Create()
        {
            ViewData["Domain"] = _tunnelConfig.Domain;

            return View();
        }

        /// <summary>
        /// 开通隧道，指定用户
        /// </summary>
        /// <returns></returns>
        [PermissionFilter]
        public async Task<IActionResult> AdminCreate(int userId)
        {
            var user = await _usersProvider.GetUserInfo(userId);

            ViewData["Domain"] = _tunnelConfig.Domain;

            return View(user);
        }

        /// <summary>
        /// 审核申请隧道
        /// </summary>
        /// <returns></returns>
        [PermissionFilter]
        public IActionResult Audit()
        {
            return View();
        }

        /// <summary>
        /// 申请隧道详情
        /// </summary>
        /// <param name="tunnelId"></param>
        /// <returns></returns>
        [PermissionFilter]
        public async Task<IActionResult> AuditDetail(string tunnelId)
        {
            var tunnel = await _tunnelProvider.GetTunnel(tunnelId);

            return View(tunnel);
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [PermissionFilter]
        [HttpGet]
        public IActionResult List(int userId)
        {
            ViewData["UserId"] = userId;

            return View();
        }
    }
}