using DDNS.Entity.Tunnel;
using DDNS.Provider.Tunnel;
using DDNS.ViewModel.Response;
using DDNS.ViewModel.Tunnel;
using DDNS.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
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
        public TunnelsApiController(TunnelProvider tunnelProvider)
        {
            _tunnelProvider = tunnelProvider;
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

            if (await _tunnelProvider.GetTunnel(vm.SubDomain) != null)
            {
                data.Code = 1;
                data.Msg = "前置域名已存在";

                return data;
            }

            var tunnel = new TunnelsEntity
            {
                UserId = Convert.ToInt32(userId),
                TunnelProtocol = vm.TunnelProtocol,
                TunnelName = vm.TunnelName,
                SubDomain = vm.SubDomain,
                LocalPort = vm.LocalPort,
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
        public async Task<ResponseViewModel<bool>> AddTunnel(int userId, TunnelsViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var tunnel = new TunnelsEntity
            {
                UserId = userId,
                TunnelProtocol = vm.TunnelProtocol,
                TunnelName = vm.TunnelName,
                SubDomain = vm.SubDomain,
                LocalPort = vm.LocalPort,
                Status = (int)TunnelStatusEnum.Audit,
                CreateTime = DateTime.Now
            };

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
        public async Task<ResponseViewModel<bool>> Edit(long tunnelId, EditTunnelViewModel vm)
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
        /// <returns></returns>
        [HttpGet]
        [Route("tunnels")]
        public async Task<IEnumerable<TunnelsEntity>> Tunnels()
        {
            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;

            return await _tunnelProvider.Tunnels(Convert.ToInt32(userId));
        }
    }
}