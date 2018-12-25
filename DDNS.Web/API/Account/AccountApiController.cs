using DDNS.Entity.AppSettings;
using DDNS.Entity.LoginLog;
using DDNS.Entity.Users;
using DDNS.Entity.Verify;
using DDNS.Provider.LoginLog;
using DDNS.Provider.Users;
using DDNS.Provider.Verify;
using DDNS.Utility;
using DDNS.ViewModel.Account;
using DDNS.ViewModel.Response;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace DDNS.Web.API.Account
{
    [Route("api")]
    [ApiController]
    public class AccountApiController : ControllerBase
    {
        private readonly UsersProvider _userProvider;
        private readonly VerifyProvider _verifyProvider;
        private readonly LoginLogProvider _loginProvider;
        private readonly IHttpContextAccessor _accessor;
        private readonly IStringLocalizer<AccountApiController> _localizer;
        private readonly EmailUtil _email;
        private readonly EmailConfig _config;

        public AccountApiController(UsersProvider usersProvider, VerifyProvider verifyProvider, LoginLogProvider loginLogProvider, IHttpContextAccessor accessor, IStringLocalizer<AccountApiController> localizer, EmailUtil email, IOptions<EmailConfig> config)
        {
            _userProvider = usersProvider;
            _verifyProvider = verifyProvider;
            _loginProvider = loginLogProvider;
            _accessor = accessor;
            _localizer = localizer;
            _email = email;
            _config = config.Value;
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

            var _user = await _userProvider.GetUserInfo(vm.UserName);
            if (_user != null)
            {
                data.Code = 1;
                data.Msg = _localizer["user"];

                return data;
            }
            _user = await _userProvider.GetUserInfo(vm.Email);
            if (_user != null)
            {
                data.Code = 1;
                data.Msg = _localizer["email"];

                return data;
            }

            var user = new UsersEntity
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Password = MD5Util.TextToMD5(vm.Password),
                RegisterTime = DateTime.Now,
                Status = (int)UserStatusEnum.UnActivated,
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

            try
            {
                _email.SendEmail(vm.UserName, vm.Email, _localizer["subject"], body);
            }
            catch (Exception e)
            {
                data.Code = 0;
                data.Msg = e.Message;
            }

            return data;
        }

        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("login")]
        public async Task<ResponseViewModel<object>> Login(LoginViewModel vm)
        {
            var data = new ResponseViewModel<object>();

            if (ModelState.IsValid)
            {
                var user = await _userProvider.GetUserInfo(vm.UserName, vm.Password);
                if (user != null)
                {
                    if (user.Status == (int)UserStatusEnum.Disable)
                    {
                        data.Code = 1;
                        data.Msg = _localizer["login.forbidden"];
                    }
                    else
                    {
                        data.Code = 0;
                        data.Msg = _localizer["login.success"];

                        var claimIdentity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                        claimIdentity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Name, user.UserName));
                        claimIdentity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimIdentity));

                        HttpContext.Session.Remove("verify_code");

                        await _loginProvider.AddLoginLog(new LoginLogEntity
                        {
                            UserId = user.Id,
                            LoginTime = DateTime.Now,
                            LoginIp = _accessor.HttpContext.Connection.RemoteIpAddress.ToString()
                        });
                    }
                }
                else
                {
                    data.Code = 1;
                    data.Msg = _localizer["login.error"];
                }
            }

            return data;
        }

        /// <summary>
        /// 忘记密码
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("forget")]
        public async Task<ResponseViewModel<object>> Forget(ForgetViewModel vm)
        {
            var data = new ResponseViewModel<object>();

            var user = await _userProvider.GetUserInfo(vm.Email);
            if (user != null)
            {
                //发送找回密码邮件
                var tempHtml = "<p>{0}</p>";
                var body = string.Empty;
                var url = _config.Domain + "/account/reset?token=" + MD5Util.TextToMD5(user.Email);
                var link = "<a href='" + url + "'>" + url + "</a>";

                body += string.Format(tempHtml, _localizer["forget.body1"]);
                body += string.Format(tempHtml, user.UserName + _localizer["forget.body2"]);
                body += string.Format(tempHtml, _localizer["forget.body3"] + link);
                body += string.Format(tempHtml, _localizer["forget.body4"]);
                body += string.Format(tempHtml, _localizer["forget.body5"]);

                data.Msg = _localizer["forget.success"];

                var verify = new VerifyEntity
                {
                    UserId = user.Id,
                    Token = MD5Util.TextToMD5(user.Email),
                    Status = (int)VerifyStatusEnum.Normal,
                    Type = (int)VerifyTypeEnum.Password,
                    Time = DateTime.Now
                };
                await _verifyProvider.AddVerify(verify);

                try
                {
                    _email.SendEmail(user.UserName, user.Email, _localizer["forget.subject"], body);
                }
                catch (Exception e)
                {
                    data.Code = 1;
                    data.Msg = e.Message;
                }
            }
            else
            {
                data.Code = 1;
                data.Msg = _localizer["forget.inexistent"];
            }

            return data;
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("reset")]
        public async Task<ResponseViewModel<bool>> Reset(ResetViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            if (vm.Password != vm.Repass)
            {
                data.Code = 1;
                data.Msg = _localizer["password"];

                return data;
            }

            var verify = _verifyProvider.GetVerifyInfo(vm.Token, VerifyTypeEnum.Password);
            if (verify != null)
            {
                var user = await _userProvider.GetUserInfo(verify.UserId);
                user.Password = MD5Util.TextToMD5(vm.Password);

                data.Code = 0;
                data.Msg = _localizer["reset.success"];
                data.Data = await _userProvider.UpdateUser(user);

                await _verifyProvider.VerifySuccess(vm.Token);
            }
            else
            {
                data.Code = 1;
                data.Msg = _localizer["reset.verify"];
            }

            return data;
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("password")]
        [Authorize]
        public async Task<ResponseViewModel<bool>> Password(PasswordViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var userId = HttpContext.User.FindFirst(u => u.Type == ClaimTypes.NameIdentifier).Value;
            var user = await _userProvider.GetUserInfo(Convert.ToInt32(userId));

            if (user != null && user.Password == MD5Util.TextToMD5(vm.OldPassword))
            {
                user.Password = MD5Util.TextToMD5(vm.Repass);

                data.Code = 0;
                data.Msg = _localizer["password.success"];
                data.Data = await _userProvider.UpdateUser(user);
            }
            else
            {
                data.Code = 1;
                data.Msg = _localizer["password.error"];
            }

            return data;
        }
    }
}