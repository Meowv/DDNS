using DDNS.Entity.Tunnel;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDNS.Interface.Tunnel
{
    public interface ITunnel
    {
        /// <summary>
        /// 开通隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        Task<bool> Create(TunnelEntity tunnel);

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        Task<bool> Edit(TunnelEntity tunnel);

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<TunnelEntity>> Tunnels(int userId);
    }
}