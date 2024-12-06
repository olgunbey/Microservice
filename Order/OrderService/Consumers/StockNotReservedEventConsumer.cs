using MassTransit;
using Microsoft.EntityFrameworkCore;
using OrderService.Database;
using Shared.Events;

namespace OrderService.Consumers
{
    public class StockNotReservedEventConsumer : IConsumer<StockNotReservedEvent>
    {
        private readonly OrderDbContext _orderDbContext;
        public StockNotReservedEventConsumer(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;

        }
        public async Task Consume(ConsumeContext<StockNotReservedEvent> context)
        {
            var order = await _orderDbContext.Order.FirstOrDefaultAsync(y => y.OrderNumber == context.Message.OrderNumber);
            order!.OrderStatus = Enums.OrderStatus.Failed;
            await _orderDbContext.SaveChangesAsync();
        }
    }
}
