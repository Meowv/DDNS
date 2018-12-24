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

        public Task<bool> AddLoginLog(LoginLogEntity log)
        {
            return _data.AddLoginLog(log);
        }
    }
}