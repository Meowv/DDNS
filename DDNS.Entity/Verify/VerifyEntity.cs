using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDNS.Entity.Verify
{
    [Table("verify")]
    public class VerifyEntity
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public string Token { get; set; }

        public int Status { get; set; }

        public int Type { get; set; }

        public DateTime Time { get; set; }
    }
}