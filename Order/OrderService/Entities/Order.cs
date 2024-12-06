using OrderService.Enums;

namespace OrderService.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public int BuyerId { get; set; }
        public Guid OrderNumber { get; set; }
        public ICollection<Product> Products { get; set; }
        public OrderStatus OrderStatus { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
