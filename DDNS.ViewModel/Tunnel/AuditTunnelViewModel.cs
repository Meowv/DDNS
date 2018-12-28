using System;

namespace DDNS.ViewModel.Tunnel
{
    public class AuditTunnelViewModel
    {
        public string TunnelId { get; set; }

        public int Status { get; set; }

        public int UserId { get; set; }

        public DateTime? ExpiredTime { get; set; }

        public int RemotePort { get; set; }
    }
}