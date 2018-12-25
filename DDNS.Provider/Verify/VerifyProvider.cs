using DDNS.DataModel.Verify;
using DDNS.Entity.Verify;
using DDNS.Interface.Verify;
using System.Threading.Tasks;

namespace DDNS.Provider.Verify
{
    public class VerifyProvider : IVerify
    {
        private readonly VerifyDataModel _data;
        public VerifyProvider(VerifyDataModel data)
        {
            _data = data;
        }

        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="verify"></param>
        /// <returns></returns>
        public Task<bool> AddVerify(VerifyEntity verify)
        {
            return _data.AddVerify(verify);
        }

        /// <summary>
        /// 验证成功
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<bool> VerifySuccess(string token)
        {
            return _data.VerifySuccess(token);
        }

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public VerifyEntity GetVerifyInfo(string token, VerifyTypeEnum type)
        {
            return _data.GetVerifyInfo(token, type);
        }
    }
}