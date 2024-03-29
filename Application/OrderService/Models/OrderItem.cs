﻿using System.ComponentModel.DataAnnotations.Schema;

namespace OrderService.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int MenuItemId { get; set; }
        public double ItemPrice { get; set; }
        [ForeignKey("OrderId")] public int OrderId { get; set; }
        public Order Order { get; set; }
    }
}