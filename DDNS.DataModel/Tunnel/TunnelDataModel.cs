using DDNS.Entity;
using DDNS.Entity.Tunnel;
using Microsoft.EntityFrameworkCore;
using System;
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
        public async Task<bool> Create(TunnelsEntity tunnel)
        {
            await _content.Tunnels.AddAsync(tunnel);
            return await _content.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 编辑隧道
        /// </summary>
        /// <param name="tunnel"></param>
        /// <returns></returns>
        public async Task<bool> Edit(TunnelsEntity tunnel)
        {
            var _tunnel = await _content.Tunnels.FirstOrDefaultAsync(t => t.TunnelId == tunnel.TunnelId);
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
        /// 获取隧道信息
        /// </summary>
        /// <param name="tunnelId"></param>
        /// <returns></returns>
        public async Task<TunnelsEntity> GetTunnel(string tunnelId)
        {
            return await _content.Tunnels.Where(t => t.TunnelId == tunnelId).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 获取隧道信息
        /// </summary>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public async Task<TunnelsEntity> GetTunnelBySubDomail(string subDomain)
        {
            return await _content.Tunnels.Where(t => t.SubDomain == subDomain).FirstOrDefaultAsync();
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<TunnelsEntity>> Tunnels(int userId)
        {
            return await _content.Tunnels.Where(t => t.UserId == userId).ToListAsync();
        }

        /// <summary>
        /// 隧道列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="subDomain"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UserTunnelsEntity>> Tunnels(string userName = null, string email = null, int status = 0, string subDomain = null)
        {
            var list = await _content.Tunnels.Join(_content.Users, t => t.UserId, u => u.Id, (t, u) => new UserTunnelsEntity
            {
                TunnelId = t.TunnelId,
                TunnelProtocol = t.TunnelProtocol,
                TunnelName = t.TunnelName,
                SubDomain = t.SubDomain,
                LocalPort = t.LocalPort,
                Status = t.Status,
                CreateTime = t.CreateTime,
                OpenTime = t.OpenTime,
                ExpiredTime = t.ExpiredTime,
                FullUrl = t.FullUrl,
                UserName = u.UserName,
                Email = u.Email,
                AuthToken = u.AuthToken,
                UserId = t.UserId
            }).ToListAsync();

            if (!string.IsNullOrEmpty(userName))
            {
                list = list.Where(x => x.UserName.Contains(userName)).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                list = list.Where(x => x.Email.Contains(email)).ToList();
            }
            if (!string.IsNullOrEmpty(subDomain))
            {
                list = list.Where(x => x.SubDomain.Contains(subDomain)).ToList();
            }
            if (status == 9)
            {
                list = list.Where(x => x.ExpiredTime <= DateTime.Now && x.ExpiredTime != null).ToList();
            }
            else
            {
                list = list.Where(x => x.Status == status && (x.ExpiredTime > DateTime.Now || x.ExpiredTime is null)).OrderBy(x => x.CreateTime).ToList();
            }

            return list;
        }
    }
}