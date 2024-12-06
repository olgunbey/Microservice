using Shared.Events.Common;
using Shared.Messages;

namespace Shared.Events
{
    public class OrderCreatedEvent : IEvent
    {
        public Guid OrderNumber { get; set; }
        public int BuyerId { get; set; }
        public List<OrderItemMessage> OrderItemMessages { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
