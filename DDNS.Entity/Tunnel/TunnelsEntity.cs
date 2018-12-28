using System;
using System.ComponentModel.DataAnnotations;

namespace DDNS.Entity.Tunnel
{
    public class TunnelsEntity
    {
        [Key]
        public string TunnelId { get; set; }

        public int UserId { get; set; }

        public string TunnelProtocol { get; set; }

        public string TunnelName { get; set; }

        public string SubDomain { get; set; }

        public string LocalPort { get; set; }

        public int Status { get; set; }

        public DateTime CreateTime { get; set; }

        public DateTime? OpenTime { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public string FullUrl { get; set; }
    }
}