namespace DDNS.Entity.AppSettings
{
    public class EmailConfig
    {
        public string Host { get; set; }

        public int Port { get; set; }

        public bool UseSsl { get; set; }

        public string UserName { get; set; }

        public string Password { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Domain { get; set; }
    }
}