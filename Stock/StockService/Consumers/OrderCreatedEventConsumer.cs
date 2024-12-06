using MassTransit;
using MongoDB.Driver;
using Shared.Constants;
using Shared.Events;
using StockService.MongoDb;

namespace StockService.Consumers
{
    public class OrderCreatedEventConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly StockMongoService _orderMongoService;
        private readonly IBus _bus;

        public OrderCreatedEventConsumer(StockMongoService orderMongoService,IBus bus)
        {
            _orderMongoService = orderMongoService;
            _bus = bus;
        }
        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            List<bool> stockResult = new();
            foreach (var item in context.Message.OrderItemMessages)
            {
                stockResult.Add((await _orderMongoService.GetCollection().FindAsync(y => y.ProductId == item.ProductId && y.Amount >= item.Count)).Any());
            }
            if (stockResult.TrueForAll(sr => sr.Equals(true)))
            {
                foreach (var item in context.Message.OrderItemMessages)
                {
                    var stock = await (await _orderMongoService.GetCollection().FindAsync(y => y.ProductId == item.ProductId)).FirstOrDefaultAsync();
                    stock.Amount -= item.Count;
                    await _orderMongoService.GetCollection().FindOneAndReplaceAsync(s => s.ProductId == item.ProductId, stock);
                }
                StockReservedEvent stockReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderNumber = context.Message.OrderNumber,
                    TotalPrice = context.Message.TotalPrice
                };
                var sendEndPoint = await _bus.GetSendEndpoint(new Uri($"queue:{RabbitMqSettings.Payment_StockReservedEventQueue}"));
                await sendEndPoint.Send(stockReservedEvent);
            }
            else
            {
                StockNotReservedEvent stockNotReservedEvent = new()
                {
                    BuyerId = context.Message.BuyerId,
                    OrderNumber = context.Message.OrderNumber
                };
                await _bus.Publish(stockNotReservedEvent);
            }
        }
    }
}
