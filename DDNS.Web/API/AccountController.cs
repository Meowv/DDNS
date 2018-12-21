using DDNS.Entity.Users;
using DDNS.Provider.Users;
using DDNS.ViewModel.Account;
using DDNS.ViewModel.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace DDNS.Web.API
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UsersProvider _provider;
        private readonly IHttpContextAccessor _accessor;

        public AccountController(UsersProvider provider, IHttpContextAccessor accessor)
        {
            _provider = provider;
            _accessor = accessor;
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
            var user = new UsersEntity
            {
                UserName = vm.UserName,
                Email = vm.Email,
                Password = vm.Password,
                RegisterTime = DateTime.Now,
                LastLoginTime = DateTime.Now,
                LastLoginIp = _accessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                Status = (int)UserStatusEnum.Unactivated,
                IsDelete = (int)UserDeleteEnum.Normal,
                IsAdmin = (int)UserTypeEnum.IsUser
            };
            var result = await _provider.AddUser(user);

            return new ResponseViewModel<bool>
            {
                Code = 0,
                Msg = "success",
                Data = result
            };
        }
    }
}