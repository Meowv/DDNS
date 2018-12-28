using System;

namespace DDNS.Entity.Users
{
    public class UsersEntity
    {
        public int Id { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        public int Status { get; set; }

        public int IsDelete { get; set; }

        public DateTime RegisterTime { get; set; }

        public int IsAdmin { get; set; }

        public string AuthToken { get; set; }
    }
}