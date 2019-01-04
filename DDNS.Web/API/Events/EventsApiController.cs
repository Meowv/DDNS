using DDNS.Entity;
using DDNS.ViewModel.Events;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace DDNS.Web.API.Events
{
    [Route("api")]
    [ApiController]
    public class EventsApiController : ControllerBase
    {
        private readonly DDNSDbContext _content;

        public EventsApiController(DDNSDbContext context)
        {
            _content = context;
        }

        /// <summary>
        /// events
        /// </summary>
        /// <param name="vm"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("events")]
        public async Task<bool> InsertData(EventsViewModel vm)
        {
            if (vm != null)
            {
                await _content.CloseConnections.AddRangeAsync(vm.CloseConnection);
                await _content.CloseTunnels.AddRangeAsync(vm.CloseTunnel);

                return await _content.SaveChangesAsync() > 0;
            }
            return false;
        }
    }
}