using OrderService.Entities;

namespace OrderService.Models
{
    public class OrderRequestDto
    {
        public int BuyerId { get; set; }
        public List<ProductRequestDto> Products { get; set; }
    }
    
}
