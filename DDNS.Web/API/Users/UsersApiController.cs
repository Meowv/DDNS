using DDNS.Entity.AppSettings;
using DDNS.Entity.Users;
using DDNS.Provider.LoginLog;
using DDNS.Provider.Users;
using DDNS.Utility;
using DDNS.ViewModel.Response;
using DDNS.ViewModel.User;
using DDNS.Web.Filter;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.Web.API.Users
{
    [Route("api")]
    [ApiController]
    [Authorize]
    [PermissionFilter]
    public class UsersApiController : ControllerBase
    {
        private readonly UsersProvider _usersProvider;
        private readonly LoginLogProvider _loginLogProvider;
        private readonly IStringLocalizer<UsersApiController> _localizer;
        private readonly TunnelConfig _tunnelConfig;

        public UsersApiController(UsersProvider usersProvider, LoginLogProvider loginLogProvider, IStringLocalizer<UsersApiController> localizer, IOptions<TunnelConfig> config)
        {
            _usersProvider = usersProvider;
            _loginLogProvider = loginLogProvider;
            _localizer = localizer;
            _tunnelConfig = config.Value;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        public async Task<ResponseViewModel<List<UsersViewModel>>> Users(int page, int limit, string userName, string email, int status,string token)
        {
            var users = new List<UsersViewModel>();

            var list = await _usersProvider.UserList(userName, email, status, token);

            var curList = list.ToList().Skip(limit * (page - 1)).Take(limit).ToList();
            curList.ForEach(x =>
            {
                var vm = new UsersViewModel
                {
                    Id = x.Id,
                    Token = x.AuthToken,
                    UserName = x.UserName,
                    Email = x.Email,
                    Status = x.Status,
                    RegisterTime = x.RegisterTime.ToLongDateString(),
                };

                users.Add(vm);
            });

            var result = new ResponseViewModel<List<UsersViewModel>>
            {
                Count = list.ToList().Count,
                Data = users
            };

            return result;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("add_user")]
        public async Task<ResponseViewModel<bool>> AddUser(EditUserViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var _user = await _usersProvider.GetUserInfo(vm.UserName);
            if (_user != null)
            {
                data.Code = 1;
                data.Msg = _localizer["username"];

                return data;
            }
            _user = await _usersProvider.GetUserInfo(vm.Email);
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
                Status = (int)UserStatusEnum.Normal,
                IsDelete = (int)UserDeleteEnum.Normal,
                IsAdmin = (int)UserTypeEnum.IsUser,
                AuthToken = GuidUtil.GetGuid()
            };

            data.Data = await _usersProvider.AddUser(user);

            return data;
        }

        /// <summary>
        /// 编辑用户
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("edit_user")]
        public async Task<ResponseViewModel<bool>> EditUser(int userId, EditUserViewModel vm)
        {
            var data = new ResponseViewModel<bool>();

            var user = await _usersProvider.GetUserInfo(userId);

            if (vm.UserName != user.UserName)
            {
                if (await _usersProvider.GetUserInfo(vm.UserName) != null)
                {
                    data.Code = 1;
                    data.Msg = _localizer["username"];

                    return data;
                }
            }
            if (vm.Email != user.Email)
            {
                if (await _usersProvider.GetUserInfo(vm.Email) != null)
                {
                    data.Code = 1;
                    data.Msg = _localizer["email"];

                    return data;
                }
            }

            user.UserName = vm.UserName;
            user.Email = vm.Email;
            if (!string.IsNullOrEmpty(vm.Password))
            {
                user.Password = MD5Util.TextToMD5(vm.Password);
            }

            data.Data = await _usersProvider.UpdateUser(user);

            return data;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("del_user")]
        public async Task<ResponseViewModel<bool>> DeleteUser(int userId)
        {
            var data = new ResponseViewModel<bool>
            {
                Data = await _usersProvider.DeleteUser(userId)
            };

            return data;
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("disable_user")]
        public async Task<ResponseViewModel<bool>> DisableUser(int userId)
        {
            var data = new ResponseViewModel<bool>
            {
                Data = await _usersProvider.DisableUser(userId)
            };

            return data;
        }

        /// <summary>
        /// 解除禁用
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("remove_disable")]
        public async Task<ResponseViewModel<bool>> RemoveDisable(int userId)
        {
            var data = new ResponseViewModel<bool>
            {
                Data = await _usersProvider.RemoveDisable(userId)
            };

            return data;
        }

        /// <summary>
        /// 重置Token
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("reset_token")]
        public async Task<ResponseViewModel<bool>> ResetToken(int userId)
        {
            var data = new ResponseViewModel<bool>();

            var user = await _usersProvider.GetUserInfo(userId);
            if (user != null)
            {
                var oldToken = user.AuthToken;
                var newToken = GuidUtil.GetGuid();

                try
                {
                    await FileUtil.ResetUserToken(_tunnelConfig.FilePath, oldToken, newToken);
                }
                catch (Exception e)
                {
                    data.Code = 1;
                    data.Msg = e.Message;

                    return data;
                }

                user.AuthToken = newToken;
            }

            data.Data = await _usersProvider.UpdateUser(user);

            return data;
        }
    }
}