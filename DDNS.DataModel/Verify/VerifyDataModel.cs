using DDNS.Entity;
using DDNS.Entity.Verify;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.DataModel.Verify
{
    public class VerifyDataModel
    {
        private readonly DDNSDbContext _content;
        public VerifyDataModel(DDNSDbContext context)
        {
            _content = context;
        }

        /// <summary>
        /// 添加验证信息
        /// </summary>
        /// <param name="verify"></param>
        /// <returns></returns>
        public async Task<bool> AddVerify(VerifyEntity verify)
        {
            await _content.Verifies.AddAsync(verify);
            return await _content.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 验证成功
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> VerifySuccess(string token)
        {
            var _verify = _content.Verifies.Where(x => x.Token == token).FirstOrDefault();
            if (_verify != null)
            {
                _verify.Status = (int)VerifyStatusEnum.Success;
                return await _content.SaveChangesAsync() > 0;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取验证信息
        /// </summary>
        /// <param name="token"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public VerifyEntity GetVerifyInfo(string token, VerifyTypeEnum type)
        {
            return _content.Verifies.Where(x => x.Token == token && x.Status == (int)VerifyStatusEnum.Normal && x.Type == (int)type).FirstOrDefault();
        }
    }
}