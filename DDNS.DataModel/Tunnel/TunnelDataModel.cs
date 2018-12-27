using DDNS.Entity;
using DDNS.Entity.Tunnel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.DataModel.Tunnel
{
    public class TunnelDataModel
    {
        private readonly DDNSDbContext _content;
        public TunnelDataModel(DDNSDbContext context)
        {
            _content = context;
        }

        /// <summary>
        /// 开通隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public async Task<bool> Create(TunnelEntity tunnel)
        {
            await _content.Tunnels.AddAsync(tunnel);
            return await _content.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public async Task<bool> Edit(TunnelEntity tunnel)
        {
            var _tunnel = await _content.Tunnels.FirstOrDefaultAsync(t => t.TunneId == tunnel.TunneId);
            if (_tunnel != null)
            {
                _tunnel = tunnel;

                return await _content.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<IEnumerable<TunnelEntity>> Tunnels(int userId)
        {
            return await _content.Tunnels.Where(t => t.UserId == userId).ToListAsync();
        }
    }
}