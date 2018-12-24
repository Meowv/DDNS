using System;

namespace DDNS.Entity.LoginLog
{
    public class LoginLogEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public DateTime LoginTime { get; set; }

        public string LoginIp { get; set; }
    }
}