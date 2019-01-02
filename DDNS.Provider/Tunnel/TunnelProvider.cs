using DDNS.DataModel.Tunnel;
using DDNS.Entity.Tunnel;
using DDNS.Interface.Tunnel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDNS.Provider.Tunnel
{
    public class TunnelProvider : ITunnel
    {
        private readonly TunnelDataModel _data;
        public TunnelProvider(TunnelDataModel data)
        {
            _data = data;
        }

        /// <summary>
        /// 开通隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public Task<bool> Create(TunnelsEntity tunnel)
        {
            return _data.Create(tunnel);
        }

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public Task<bool> Edit(TunnelsEntity tunnel)
        {
            return _data.Edit(tunnel);
        }

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="tunnelId"></param>
        /// <returns></returns>
        public Task<TunnelsEntity> GetTunnel(string tunnelId)
        {
            return _data.GetTunnel(tunnelId);
        }

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public Task<TunnelsEntity> GetTunnelBySubDomail(string subDomain)
        {
            return _data.GetTunnelBySubDomail(subDomain);
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TunnelsEntity>> Tunnels(int userId)
        {
            return _data.Tunnels(userId);
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public Task<IEnumerable<UserTunnelsEntity>> Tunnels(string userName = null, string email = null, int status = 0, string subDomain = null)
        {
            return _data.Tunnels(userName, email, status, subDomain);
        }
    }
}