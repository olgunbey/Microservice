﻿namespace OrderService.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public int Count { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }
    }
}
