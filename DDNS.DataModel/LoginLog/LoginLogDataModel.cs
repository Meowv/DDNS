using DDNS.Entity;
using DDNS.Entity.LoginLog;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.DataModel.LoginLog
{
    public class LoginLogDataModel
    {
        private readonly DDNSDbContext _content;
        public LoginLogDataModel(DDNSDbContext context)
        {
            _content = context;
        }

        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public async Task<bool> AddLoginLog(LoginLogEntity log)
        {
            await _content.LoginLog.AddAsync(log);
            return await _content.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 获取上次登录日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<LoginLogEntity> GetLastLoginLog(int userId)
        {
            return await _content.LoginLog.Where(x => x.UserId == userId).OrderByDescending(x => x.LoginTime).Take(2).OrderBy(x => x.LoginTime).Take(1).FirstOrDefaultAsync();
        }
    }
}