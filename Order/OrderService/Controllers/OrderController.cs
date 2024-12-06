using MassTransit;
using Microsoft.AspNetCore.Mvc;
using OrderService.Database;
using OrderService.Entities;
using OrderService.Models;
using Shared.Events;

namespace OrderService.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly OrderDbContext _orderDbContext;
        private readonly IBus _bus;
        public OrderController(OrderDbContext orderDbContext, IBus bus)
        {
            _orderDbContext = orderDbContext;
            _bus = bus;
        }
        [HttpPost]
        public async Task<IActionResult> OrderSend(OrderRequestDto orderRequestDtos)
        {
            if (orderRequestDtos.BuyerId == 0)
                return Ok("BuyerId giriniz");

            var order = new Order()
            {
                BuyerId = orderRequestDtos.BuyerId,
                Products = orderRequestDtos.Products.Select(y => new Product()
                {
                    Count = y.Count,
                    ProductId = y.ProductId,
                    Price = y.Price,
                }).ToList(),
                OrderNumber=Guid.NewGuid(),
                OrderStatus = Enums.OrderStatus.Suspend,
                TotalPrice=orderRequestDtos.Products.Sum(y=>y.Count*y.Price)

            };
            _orderDbContext.Order.Add(order);
            await _orderDbContext.SaveChangesAsync();

            var orderCreatedEvent = new OrderCreatedEvent()
            {
                BuyerId=orderRequestDtos.BuyerId,
                OrderNumber=order.OrderNumber,
                OrderItemMessages = orderRequestDtos.Products.Select(y=> new Shared.Messages.OrderItemMessage()
                {
                    Count=y.Count,
                    ProductId=y.ProductId,
                }).ToList(),
                TotalPrice = order.TotalPrice
            };
            await _bus.Publish(orderCreatedEvent);
            return Ok();
        }
    }
}
