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

        /// <summary>
        /// 获取上次登录日志
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<LoginLogEntity> GetLastLoginLog(int userId);
    }
}