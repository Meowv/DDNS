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

        public Task<bool> AddVerify(VerifyEntity verify)
        {
            return _data.AddVerify(verify);
        }

        public Task<bool> VerifySuccess(string token)
        {
            return _data.VerifySuccess(token);
        }

        public VerifyEntity GetVerifyInfo(string token)
        {
            return _data.GetVerifyInfo(token);
        }
    }
}