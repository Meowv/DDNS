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
        Task<TunnelsEntity> GetTunnel(string tunnelId);

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        Task<TunnelsEntity> GetTunnelBySubDomail(string subDomain);

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        Task<IEnumerable<TunnelsEntity>> Tunnels(int userId);

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        Task<IEnumerable<UserTunnelsEntity>> Tunnels(string userName = null, string email = null, int status = 0, string subDomain = null);
    }
}