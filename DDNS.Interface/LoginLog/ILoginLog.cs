using DDNS.Entity.LoginLog;
using System.Threading.Tasks;

namespace DDNS.Interface.LoginLog
{
    public interface ILoginLog
    {
        /// <summary>
        /// 添加登录信息
        /// </summary>
        /// <param name="log"></param>
        /// <returns></returns>
        Task<bool> AddLoginLog(LoginLogEntity log);
    }
}