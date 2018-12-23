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
        /// <param name="email"></param>
        /// <returns></returns>
        Task<UsersEntity> GetUserInfo(string email);

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
        /// <returns></returns>
        Task<IEnumerable<UsersEntity>> UserList();
    }
}