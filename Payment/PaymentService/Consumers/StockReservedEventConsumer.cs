using MassTransit;
using PaymentService.Database;
using Shared.Constants;
using Shared.Events;

namespace PaymentService.Consumers
{
    public class StockReservedEventConsumer : IConsumer<StockReservedEvent>
    {
        private readonly PaymentDbContext _paymentDbContext;
        private readonly IBus _bus;
        public StockReservedEventConsumer(PaymentDbContext paymentDbContext, IBus bus)
        {
            _paymentDbContext = paymentDbContext;
            _bus = bus;
        }

        public async Task Consume(ConsumeContext<StockReservedEvent> context)
        {
            var PaymentUser = await _paymentDbContext.PaymentUser.FindAsync(context.Message.BuyerId);

            if (PaymentUser!.Balance > context.Message.TotalPrice)
            {
                PaymentUser.Balance -= context.Message.TotalPrice;

                await _paymentDbContext.SaveChangesAsync();
                PaymentCompletedEvent paymentCompletedEvent = new()
                {
                    OrderNumber = context.Message.OrderNumber,
                };

                var sendEndPoint = await _bus.GetSendEndpoint(new Uri($"queue:{RabbitMqSettings.Order_PaymentCompletedEventQueue}"));
                await sendEndPoint.Send(paymentCompletedEvent);
            }
            else
            {
                PaymentNotCompletedEvent paymentNotCompletedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderNumber = context.Message.OrderNumber,
                };
                await _bus.Publish(paymentNotCompletedEvent);

            }

        }
    }
}
