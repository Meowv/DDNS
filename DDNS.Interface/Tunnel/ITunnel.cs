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
        Task<bool> Create(TunnelsEntity tunnel);

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        Task<bool> Edit(TunnelsEntity tunnel);

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="tunnelId"></param>
        /// <returns></returns>
        Task<TunnelsEntity> GetTunnel(long tunnelId);

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        Task<TunnelsEntity> GetTunnel(string subDomain);

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TunnelsEntity>> Tunnels(int userId);
    }
}