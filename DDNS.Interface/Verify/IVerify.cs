using DDNS.Entity.Verify;
using System.Threading.Tasks;

namespace DDNS.Interface.Verify
{
    public interface IVerify
    {
        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="verify"></param>
        /// <returns></returns>
        Task<bool> AddVerify(VerifyEntity verify);

        /// <summary>
        /// 验证成功
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<bool> VerifySuccess(string token);

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        VerifyEntity GetVerifyInfo(string token, VerifyTypeEnum type);
    }
}