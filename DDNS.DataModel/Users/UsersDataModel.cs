using DDNS.Entity;
using DDNS.Entity.Users;
using DDNS.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DDNS.DataModel.Users
{
    public class UsersDataModel
    {
        private readonly DDNSDbContext _content;
        public UsersDataModel(DDNSDbContext context)
        {
            _content = context;
        }

        /// <summary>
        /// 添加用户
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> AddUser(UsersEntity user)
        {
            await _content.Users.AddAsync(user);
            return await _content.SaveChangesAsync() > 0;
        }

        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DeleteUser(int id)
        {
            var _user = await _content.Users.FindAsync(id);
            if (_user != null)
            {
                _user.IsDelete = (int)UserDeleteEnum.Deleted;
                return await _content.SaveChangesAsync() > 0;
            }
            else
                return false;
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<bool> DisableUser(int id)
        {
            var _user = await _content.Users.FindAsync(id);
            if (_user != null)
            {
                _user.Status = (int)UserStatusEnum.Disable;
                return await _content.SaveChangesAsync() > 0;
            }
            else
                return false;
        }

        /// <summary>
        /// 更新用户信息
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task<bool> UpdateUser(UsersEntity user)
        {
            var _user = await _content.Users.FindAsync(user.Id);
            if (_user != null)
            {
                _user = user;

                return await _content.SaveChangesAsync() > 0;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UsersEntity> GetUserInfo(int id)
        {
            return await _content.Users.FindAsync(id);
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<UsersEntity> GetUserInfo(string userName, string password)
        {
            return await _content.Users.FirstOrDefaultAsync(u => (u.UserName == userName || u.Email == userName) && u.Password == MD5Util.TextToMD5(password));
        }

        /// <summary>
        /// 获取用户信息
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<UsersEntity> GetUserInfo(string userName)
        {
            return await _content.Users.FirstOrDefaultAsync(u => u.UserName == userName || u.Email == userName);
        }

        /// <summary>
        /// 用户列表
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="email"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<IEnumerable<UsersEntity>> UserList(string userName = null, string email = null, int status = 0)
        {
            var list = await _content.Users.Where(x => x.IsDelete == (int)UserDeleteEnum.Normal && x.IsAdmin == (int)UserTypeEnum.IsUser).ToListAsync();

            if (!string.IsNullOrEmpty(userName))
            {
                list = list.Where(x => x.UserName == userName).ToList();
            }
            if (!string.IsNullOrEmpty(email))
            {
                list = list.Where(x => x.Email == email).ToList();
            }
            if (status == 0)
            {
                list = list.Where(x => x.Status == status).ToList();
            }

            return list;
        }
    }
}