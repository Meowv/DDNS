using DDNS.DataModel.LoginLog;
using DDNS.Entity.LoginLog;
using DDNS.Interface.LoginLog;
using System.Threading.Tasks;

namespace DDNS.Provider.LoginLog
{
    public class LoginLogProvider : ILoginLog
    {
        private readonly LoginLogDataModel _data;

        public LoginLogProvider(LoginLogDataModel data)
        {
            _data = data;
        }

        /// <summary>
        /// 添加登录日志
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        public Task<bool> AddLoginLog(LoginLogEntity log)
        {
            return _data.AddLoginLog(log);
        }

        /// <summary>
        /// 获取上次登录日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public Task<LoginLogEntity> GetLastLoginLog(int userId)
        {
            return _data.GetLastLoginLog(userId);
        }
    }
}