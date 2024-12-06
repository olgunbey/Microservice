using Shared.Events.Common;

namespace Shared.Events
{
    public class StockNotReservedEvent : IEvent
    {
        public Guid OrderNumber { get; set; }
        public int BuyerId { get; set; }
    }
}
