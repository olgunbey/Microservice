using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using Shared.Events;

namespace OrderService.Consumers
{
    public class PaymentCompletedEventConsumer : IConsumer<PaymentCompletedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        public PaymentCompletedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }
        public async Task Consume(ConsumeContext<PaymentCompletedEvent> context)
        {
            var order = await _orderDbContext.Order.FirstOrDefaultAsync(y => y.OrderNumber == context.Message.OrderNumber);
            order!.OrderStatus=Enums.OrderStatus.Completed;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
