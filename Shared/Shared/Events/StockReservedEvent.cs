using Shared.Events.Common;

namespace Shared.Events
{
    public class StockReservedEvent : IEvent
    {
        public Guid OrderNumber { get; set; }
        public int BuyerId { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
