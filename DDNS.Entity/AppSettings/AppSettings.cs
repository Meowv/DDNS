using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;
using System.Data;

namespace DDNS.Entity.AppSettings
{
    public class AppSettings
    {
        public ConnectionStrings ConnectionStrings { get; }

        public AppSettings(IOptions<ConnectionStrings> connectionStrings)
        {
            ConnectionStrings = connectionStrings.Value;
        }

        public IDbConnection DDNSConnection
        {
            get
            {
                return new MySqlConnection(ConnectionStrings.DDNSConnection);
            }
        }
    }
    public class ConnectionStrings
    {
        public string DDNSConnection { get; set; }
    }
}