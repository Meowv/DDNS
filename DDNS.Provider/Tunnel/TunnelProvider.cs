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
        public Task<TunnelsEntity> GetTunnel(long tunnelId)
        {
            return _data.GetTunnel(tunnelId);
        }

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public Task<TunnelsEntity> GetTunnel(string subDomain)
        {
            return _data.GetTunnel(subDomain);
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        public Task<IEnumerable<TunnelsEntity>> Tunnels(int userId)
        {
            return _data.Tunnels(userId);
        }
    }
}