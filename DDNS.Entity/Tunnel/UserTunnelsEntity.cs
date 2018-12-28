namespace DDNS.Entity.Tunnel
{
    public class UserTunnelsEntity : TunnelsEntity
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string AuthToken { get; set; }
    }
}