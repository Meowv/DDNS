using DDNS.DataModel.Users;
using DDNS.Entity.Users;
using DDNS.Interface.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDNS.Provider.Users
{
    public class UsersProvider : IUsers
    {
        private readonly UsersDataModel _data;

        public UsersProvider(UsersDataModel data)
        {
            _data = data;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> AddUser(UsersEntity user)
        {
            return _data.AddUser(user);
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DeleteUser(int id)
        {
            return _data.DeleteUser(id);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> DisableUser(int id)
        {
            return _data.DisableUser(id);
        }

        /// <summary>
        /// 解除禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<bool> RemoveDisable(int id)
        {
            return _data.RemoveDisable(id);
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Task<bool> UpdateUser(UsersEntity user)
        {
            return _data.UpdateUser(user);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public Task<UsersEntity> GetUserInfo(int id)
        {
            return _data.GetUserInfo(id);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public Task<UsersEntity> GetUserInfo(string userName)
        {
            return _data.GetUserInfo(userName);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Task<UsersEntity> GetUserInfo(string userName, string password)
        {
            return _data.GetUserInfo(userName, password);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<IEnumerable<UsersEntity>> UserList(string userName = null, string email = null, int status = 0, string token = null)
        {
            return _data.UserList(userName, email, status, token);
        }
    }
}