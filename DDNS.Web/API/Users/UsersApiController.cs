using DDNS.Provider.LoginLog;
using DDNS.Provider.Users;
using DDNS.ViewModel.Response;
using DDNS.ViewModel.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.Web.API.Users
{
    [Route("api")]
    [ApiController]
    [Authorize]
    public class UsersApiController : ControllerBase
    {
        private readonly UsersProvider _usersProvider;
        private readonly LoginLogProvider _loginLogProvider;

        public UsersApiController(UsersProvider usersProvider, LoginLogProvider loginLogProvider)
        {
            _usersProvider = usersProvider;
            _loginLogProvider = loginLogProvider;
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="page"></param>
        /// <param name="limit"></param>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("users")]
        public async Task<ResponseViewModel<List<UsersViewModel>>> Users(int page, int limit, string userName, string email, int status)
        {
            var users = new List<UsersViewModel>();

            var list = await _usersProvider.UserList(userName, email, status);

            var curList = list.ToList().Skip(limit * (page - 1)).Take(limit).ToList();
            curList.ForEach(x =>
            {
                var vm = new UsersViewModel
                {
                    Id = x.Id,
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
    }
}