using Shared.Events.Common;

namespace Shared.Events
{
    public class PaymentNotCompletedEvent : IEvent
    {
        public int BuyerId { get; set; }
        public Guid OrderNumber { get; set; }
    }
}
