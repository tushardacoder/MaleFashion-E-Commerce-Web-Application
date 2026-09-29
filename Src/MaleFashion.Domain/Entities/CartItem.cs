using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class CartItem 
    {
        public Guid Id { get; set; }

        // =====================================
        // Cart
        // =====================================

        public Guid CartId { get; set; }

        public Cart Cart { get; set; } = default!;


        // =====================================
        // Product Variant
        // =====================================

        public Guid ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; }
            = default!;


        // =====================================
        // Quantity
        // =====================================

        public int Quantity { get; set; }


        // =====================================
        // Price Snapshot
        // =====================================

        public decimal UnitPrice { get; set; }

    }
}
