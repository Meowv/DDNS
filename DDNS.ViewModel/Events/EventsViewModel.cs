using DDNS.Entity.Events;
using System.Collections.Generic;

namespace DDNS.ViewModel.Events
{
    public class EventsViewModel
    {
        public List<CloseConnectionEntity> CloseConnection { get; set; }

        public List<CloseTunnelEntity> CloseTunnel { get; set; }
    }
}