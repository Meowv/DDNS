using DDNS.Entity;
using DDNS.Entity.LoginLog;
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

        public async Task<bool> AddLoginLog(LoginLogEntity log)
        {
            await _content.LoginLog.AddAsync(log);
            return await _content.SaveChangesAsync() > 0;
        }
    }
}