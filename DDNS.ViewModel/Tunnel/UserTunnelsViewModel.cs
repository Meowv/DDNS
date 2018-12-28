namespace DDNS.ViewModel.Tunnel
{
    public class UserTunnelsViewModel
    {
        public string TunnelId { get; set; }

        public string UserName { get; set; }

        public string Email { get; set; }

        public string AuthToken { get; set; }

        public string TunnelProtocol { get; set; }

        public string TunnelName { get; set; }

        public string SubDomain { get; set; }

        public string LocalPort { get; set; }

        public string CreateTime { get; set; }

        public string OpenTime { get; set; }

        public string ExpiredTime { get; set; }

        public string FullUrl { get; set; }

        public int Status { get; set; }

        public int UserId { get; set; }
    }
}