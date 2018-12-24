using System;

namespace DDNS.ViewModel.User
{
    public class UserInfoViewModel
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public DateTime RegisterTime { get; set; }

        public DateTime LastLoginTime { get; set; }

        public string LastLoginIP { get; set; }
    }
}