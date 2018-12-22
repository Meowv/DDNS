using DDNS.Entity.AppSettings;
using DDNS.Entity.Users;
using DDNS.Entity.Verify;
using DDNS.Provider.Users;
using DDNS.Provider.Verify;
using DDNS.Utility;
using DDNS.ViewModel.Account;
using DDNS.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using System;
using System.Threading.Tasks;

namespace DDNS.Web.API.Account
{
    [Route("api")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UsersProvider _userProvider;
        private readonly VerifyProvider _verifyProvider;
        private readonly IHttpContextAccessor _accessor;
        private readonly IStringLocalizer<AccountApiController> _localizer;
        private readonly EmailUtil _email;
        private readonly EmailConfig _config;

        public AccountApiController(UsersProvider usersProvider, VerifyProvider verifyProvider, IHttpContextAccessor accessor, IStringLocalizer<AccountApiController> localizer, EmailUtil email, EmailConfig config)
        {
            _userProvider = usersProvider;
            _verifyProvider = verifyProvider;
            _accessor = accessor;
            _localizer = localizer;
            _email = email;
            _config = config;
        }

        /// <summary>
        /// 用户注册
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("register")]
        public async Task<ResponseViewModel<bool>> Register(RegisterViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            if (vm.Password != vm.Repass)
            {
                data.Code = 1;
                data.Msg = _localizer["password"];

                return data;
            }

            var user = new UsersEntity
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Password = MD5Util.TextToMD5(vm.Password),
                RegisterTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                LastLoginIp = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Status = (int)UserStatusEnum.Unactivated,
                IsDelete = (int)UserDeleteEnum.Normal,
                IsAdmin = (int)UserTypeEnum.IsUser
            };

            data.Data = await _userProvider.AddUser(user);
            data.Msg = _localizer["success"];

            var verify = new VerifyEntity
            {
                UserId = _userProvider.GetUserInfo(vm.Email).Id,
                Token = MD5Util.TextToMD5(vm.Email),
                Status = (int)VerifyStatusEnum.Normal,
                Type = (int)VerifyTypeEnum.Register,
                Time = DateTime.Now
            };
            await _verifyProvider.AddVerify(verify);

            //发送激活邮件
            var tempHtml = "<p>{0}</p>";
            var body = string.Empty;
            var url = _config.Domain + "/account/verify?token=" + MD5Util.TextToMD5(vm.Email);
            var link = "<a href='" + url + "'>" + url + "</a>";

            body += string.Format(tempHtml, _localizer["body1"]);
            body += string.Format(tempHtml, vm.UserName + _localizer["body2"]);
            body += string.Format(tempHtml, _localizer["body3"] + link);
            body += string.Format(tempHtml, _localizer["body4"]);
            body += string.Format(tempHtml, _localizer["body5"]);

            _email.SendEmail(vm.UserName, vm.Email, _localizer["subject"], body);

            return data;
        }
    }
}