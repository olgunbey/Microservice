using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Constants
{
    public class RabbitMqSettings
    {
        public const string Stock_OrderCreatedEventQueue = "stock-order-created-event-queue";
        public const string Payment_StockReservedEventQueue = "order-stock-reserved-event-queue";
        public const string Order_PaymentCompletedEventQueue= "order-payment-completed-event-queue";
        public const string Order_StockNotCompletedEventQueue = "order-stock-notcompleted-event-queue";
        public const string Order_PaymentNotCompletedEventQueue = "order-payment-notcompleted-event-queue";


    }
}
