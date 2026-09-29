using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class OrderItem 
    {
        public Guid Id { get; set; }


        // =====================================
        // Order
        // =====================================

        public Guid OrderId { get; set; }

        public Order Order { get; set; } = default!;


        // =====================================
        // Product Variant Reference
        // =====================================

        public Guid ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; }
            = default!;


        // =====================================
        // Product Snapshot
        // =====================================

        public string ProductName { get; set; } = default!;

        public string Color { get; set; } = default!;

        public string Size { get; set; } = default!;

        public string Sku { get; set; } = default!;


        // =====================================
        // Price Snapshot
        // =====================================

        public decimal UnitPrice { get; set; }


        // =====================================
        // Quantity
        // =====================================

        public int Quantity { get; set; }


        // =====================================
        // Line Total
        // =====================================

        public decimal TotalPrice { get; set; }
    }
}
