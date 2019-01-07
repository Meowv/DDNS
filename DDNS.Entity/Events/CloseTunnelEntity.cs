using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DDNS.Entity.Events
{
    [Table("closetunnel")]
    public class CloseTunnelEntity
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public string OS { get; set; }
        public string ClientId { get; set; }
        public string Protocol { get; set; }
        public string Url { get; set; }
        public string User { get; set; }
        public string Version { get; set; }
        public string Reason { get; set; }
        public float Duration { get; set; }
        public bool HttpAuth { get; set; }
        public string Subdomain { get; set; }
        public int RemotePort { get; set; }
        public string ReqId { get; set; }
    }
}