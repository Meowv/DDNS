using DDNS.Entity;
using DDNS.Entity.Users;
using DDNS.Utility;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        public async Task<bool> AddUser(UsersEntity user)
        {
            await _content.Users.AddAsync(user);
            return await _content.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteUser(int id)
        {
            var _user = await _content.Users.FindAsync(id);
            if (_user != null)
            {
                _user.IsDelete = 1;
                return await _content.SaveChangesAsync() > 0;
            }
            else
                return false;
        }

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

        public async Task<UsersEntity> GetUserInfo(int id)
        {
            return await _content.Users.FindAsync(id);
        }

        public async Task<UsersEntity> GetUserInfo(string userName, string password)
        {
            return await _content.Users.FirstOrDefaultAsync(u => (u.UserName == userName || u.Email == userName) && u.Password == MD5Util.TextToMD5(password));
        }

        public async Task<UsersEntity> GetUserInfo(string email)
        {
            return await _content.Users.FindAsync(email);
        }

        public async Task<IEnumerable<UsersEntity>> UserList()
        {
            return await _content.Users.ToListAsync();
        }
    }
}