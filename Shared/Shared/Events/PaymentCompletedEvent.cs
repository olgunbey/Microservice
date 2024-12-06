using Shared.Events.Common;

namespace Shared.Events
{
    public class PaymentCompletedEvent : IEvent
    {
        public Guid OrderNumber { get; set; }

    }
}
