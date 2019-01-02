using DDNS.Entity.AppSettings;
using DDNS.Entity.Tunnel;
using DDNS.Provider.Tunnel;
using DDNS.Provider.Users;
using DDNS.Utility;
using DDNS.ViewModel.Response;
using DDNS.ViewModel.Tunnel;
using DDNS.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DDNS.Web.API.Tunnels
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class TunnelsApiController : ControllerBase
    {
        private readonly TunnelProvider _tunnelProvider;
        private readonly UsersProvider _usersProvider;
        private readonly TunnelConfig _tunnelConfig;
        private readonly IStringLocalizer<TunnelsApiController> _localizer;

        public TunnelsApiController(TunnelProvider tunnelProvider, UsersProvider usersProvider, IOptions<TunnelConfig> config, IStringLocalizer<TunnelsApiController> localizer)
        {
            _tunnelProvider = tunnelProvider;
            _usersProvider = usersProvider;
            _tunnelConfig = config.Value;
            _localizer = localizer;
        }

        /// <summary>
        /// 添加隧道
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add_tunnel")]
        public async Task<ResponseViewModel<bool>> AddTunnel(TunnelsViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;

            if (await _tunnelProvider.GetTunnelBySubDomail(vm.SubDomain) != null)
            {
                data.Code = 1;
                data.Msg = _localizer["add_tunnel.msg"];

                return data;
            }

            var tunnelId = GuidUtil.GenerateGuid();
            if (await _tunnelProvider.GetTunnel(tunnelId) != null)
            {
                data.Code = 1;
                data.Msg = _localizer["add_tunnel.error"];

                return data;
            }

            var tunnel = new TunnelsEntity
            {
                TunnelId = tunnelId,
                UserId = Convert.ToInt32(userId),
                TunnelProtocol = vm.TunnelProtocol,
                TunnelName = vm.TunnelName,
                SubDomain = vm.SubDomain,
                LocalPort = vm.LocalPort,
                FullUrl = vm.FullUrl,
                Status = (int)TunnelStatusEnum.Audit,
                CreateTime = DateTime.Now
            };

            data.Data = await _tunnelProvider.Create(tunnel);

            return data;
        }

        /// <summary>
        /// 添加隧道，指定用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("v2/add_tunnel")]
        [PermissionFilter]
        public async Task<ResponseViewModel<bool>> AddTunnel(int userId, AdminTunnelsViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            if (await _tunnelProvider.GetTunnelBySubDomail(vm.SubDomain) != null)
            {
                data.Code = 1;
                data.Msg = _localizer["add_tunnel.msg"];

                return data;
            }

            var tunnelId = GuidUtil.GenerateGuid();
            if (await _tunnelProvider.GetTunnel(tunnelId) != null)
            {
                data.Code = 1;
                data.Msg = _localizer["add_tunnel.error"];

                return data;
            }

            var tunnel = new TunnelsEntity
            {
                TunnelId = tunnelId,
                UserId = userId,
                TunnelProtocol = vm.TunnelProtocol,
                TunnelName = vm.TunnelName,
                SubDomain = vm.SubDomain,
                LocalPort = vm.LocalPort,
                Status = (int)TunnelStatusEnum.Pass,
                CreateTime = DateTime.Now,
                OpenTime = DateTime.Now,
                ExpiredTime = vm.ExpiredTime,
                FullUrl = vm.FullUrl
            };

            var user = await _usersProvider.GetUserInfo(userId);

            try
            {
                await FileUtil.WriteTunnel(tunnel, user, _tunnelConfig.FilePath, vm.RemotePort);
            }
            catch (Exception e)
            {
                data.Code = 1;
                data.Msg = e.Message;

                return data;
            }

            data.Data = await _tunnelProvider.Create(tunnel);

            return data;
        }

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnelId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("edit_tunnel")]
        public async Task<ResponseViewModel<bool>> Edit(string tunnelId, EditTunnelViewModel vm)
        {
            var data = new ResponseViewModel<bool>();
            var tunnel = await _tunnelProvider.GetTunnel(tunnelId);
            if (tunnel != null)
            {
                tunnel.TunnelName = vm.TunnelName;

                data.Data = await _tunnelProvider.Edit(tunnel);
            }

            return data;
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("tunnels")]
        public async Task<ResponseViewModel<List<UserTunnelsViewModel>>> Tunnels(int page, int limit)
        {
            var tunnels = new List<UserTunnelsViewModel>();

            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;

            var list = await _tunnelProvider.Tunnels(Convert.ToInt32(userId));

            var curList = list.ToList().Skip(limit * (page - 1)).Take(limit).ToList();
            curList.ForEach(x =>
            {
                var status = (x.Status == 1 && x.ExpiredTime <= DateTime.Now && x.ExpiredTime != null) ? 9 : x.Status;

                var vm = new UserTunnelsViewModel
                {
                    TunnelId = x.TunnelId,
                    UserId = x.UserId,
                    TunnelProtocol = x.TunnelProtocol,
                    TunnelName = x.TunnelName,
                    SubDomain = x.SubDomain,
                    LocalPort = x.LocalPort,
                    Status = status,
                    CreateTime = x.CreateTime.ToLongDateString(),
                    OpenTime = x.OpenTime == null ? null : Convert.ToDateTime(x.OpenTime).ToLongDateString(),
                    ExpiredTime = x.ExpiredTime == null ? null : Convert.ToDateTime(x.ExpiredTime).ToLongDateString(),
                    FullUrl = x.FullUrl
                };
                tunnels.Add(vm);
            });

            var result = new ResponseViewModel<List<UserTunnelsViewModel>>
            {
                Count = list.ToList().Count,
                Data = tunnels
            };

            return result;
        }

        /// <summary>
        /// 隧道列表，指定用户
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("tunnel_list")]
        public async Task<ResponseViewModel<List<UserTunnelsViewModel>>> TunnelList(int page, int limit, int userId)
        {
            var tunnels = new List<UserTunnelsViewModel>();

            var list = await _tunnelProvider.Tunnels(userId);

            var curList = list.ToList().Skip(limit * (page - 1)).Take(limit).ToList();
            curList.ForEach(x =>
            {
                var status = (x.Status == 1 && x.ExpiredTime <= DateTime.Now && x.ExpiredTime != null) ? 9 : x.Status;

                var vm = new UserTunnelsViewModel
                {
                    TunnelId = x.TunnelId,
                    UserId = x.UserId,
                    TunnelProtocol = x.TunnelProtocol,
                    TunnelName = x.TunnelName,
                    SubDomain = x.SubDomain,
                    LocalPort = x.LocalPort,
                    Status = status,
                    CreateTime = x.CreateTime.ToLongDateString(),
                    OpenTime = x.OpenTime == null ? null : Convert.ToDateTime(x.OpenTime).ToLongDateString(),
                    ExpiredTime = x.ExpiredTime == null ? null : Convert.ToDateTime(x.ExpiredTime).ToLongDateString(),
                    FullUrl = x.FullUrl
                };
                tunnels.Add(vm);
            });

            var result = new ResponseViewModel<List<UserTunnelsViewModel>>
            {
                Count = list.ToList().Count,
                Data = tunnels
            };

            return result;
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        /// <returns></returns>
        [HttpGet]
        [Route("v2/tunnels")]
        [PermissionFilter]
        public async Task<ResponseViewModel<List<UserTunnelsViewModel>>> Tunnels(int page, int limit, string userName, string email, int status,string subDomain)
        {
            var tunnels = new List<UserTunnelsViewModel>();

            var list = await _tunnelProvider.Tunnels(userName, email, status, subDomain);

            var curList = list.ToList().Skip(limit * (page - 1)).Take(limit).ToList();
            curList.ForEach(x =>
            {
                var vm = new UserTunnelsViewModel
                {
                    TunnelId = x.TunnelId,
                    UserName = x.UserName,
                    Email = x.Email,
                    AuthToken = x.AuthToken,
                    TunnelProtocol = x.TunnelProtocol,
                    TunnelName = x.TunnelName,
                    SubDomain = x.SubDomain,
                    LocalPort = x.LocalPort,
                    CreateTime = x.CreateTime.ToLongDateString(),
                    OpenTime = x.OpenTime == null ? null : Convert.ToDateTime(x.OpenTime).ToLongDateString(),
                    ExpiredTime = x.ExpiredTime == null ? null : Convert.ToDateTime(x.ExpiredTime).ToLongDateString(),
                    FullUrl = x.FullUrl,
                    Status = x.Status,
                    UserId = x.UserId
                };
                tunnels.Add(vm);
            });

            var data = new ResponseViewModel<List<UserTunnelsViewModel>>
            {
                Count = list.Count(),
                Data = tunnels
            };
            return data;
        }

        /// <summary>
        /// 审核用户申请隧道
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("audit_tunnel")]
        [PermissionFilter]
        public async Task<ResponseViewModel<bool>> AuditTunnel(AuditTunnelViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var user = await _usersProvider.GetUserInfo(vm.UserId);
            var tunnel = await _tunnelProvider.GetTunnel(vm.TunnelId);

            if (tunnel != null && user != null)
            {
                tunnel.Status = vm.Status;

                if (vm.Status == 1)
                {
                    tunnel.OpenTime = DateTime.Now;
                    tunnel.ExpiredTime = vm.ExpiredTime;
                    try
                    {
                        await FileUtil.WriteTunnel(tunnel, user, _tunnelConfig.FilePath, vm.RemotePort);
                    }
                    catch (Exception e)
                    {
                        data.Code = 1;
                        data.Msg = e.Message;

                        return data;
                    }
                }

                data.Data = await _tunnelProvider.Edit(tunnel);
            }

            return data;
        }
    }
}