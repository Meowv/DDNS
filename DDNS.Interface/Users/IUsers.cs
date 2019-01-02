using DDNS.Entity.Users;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DDNS.Interface.Users
{
    public interface IUsers
    {
        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> AddUser(UsersEntity user);

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DeleteUser(int id);

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> DisableUser(int id);

        /// <summary>
        /// 解除禁用
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> RemoveDisable(int id);

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task<bool> UpdateUser(UsersEntity user);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UsersEntity> GetUserInfo(int id);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        Task<UsersEntity> GetUserInfo(string userName);

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<UsersEntity> GetUserInfo(string userName, string password);

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<IEnumerable<UsersEntity>> UserList(string userName = null, string email = null, int status = 0, string token = null);
    }
}