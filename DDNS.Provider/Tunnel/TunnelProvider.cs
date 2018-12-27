using DDNS.DataModel.Tunnel;
using DDNS.Entity.Tunnel;
using DDNS.Interface.Tunnel;
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
        public Task<bool> Create(TunnelEntity tunnel)
        {
            return _data.Create(tunnel);
        }

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public Task<bool> Edit(TunnelEntity tunnel)
        {
            return _data.Edit(tunnel);
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<IEnumerable<TunnelEntity>> Tunnels(int userId)
        {
            return _data.Tunnels(userId);
        }
    }
}