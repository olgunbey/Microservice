using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using Shared.Events;

namespace OrderService.Consumers
{
    public class PaymentNotCompletedEventConsumer : IConsumer<PaymentNotCompletedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        public PaymentNotCompletedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentNotCompletedEvent> context)
        {
            var order = await _orderDbContext.Order.SingleOrDefaultAsync(y => y.OrderNumber == context.Message.OrderNumber);
            order!.OrderStatus = Enums.OrderStatus.Failed;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
