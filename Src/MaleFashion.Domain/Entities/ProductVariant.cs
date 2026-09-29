using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class ProductVariant 
    {
        public Guid Id { get; set; }


        // =====================================
        // Product Relationship
        // =====================================

        public Guid ProductId { get; set; }

        public Product Product { get; set; } = default!;


        // =====================================
        // Variant Information
        // =====================================

        public string Sku { get; set; } = default!;

        public string Size { get; set; } = default!;

        public string Color { get; set; } = default!;


        // =====================================
        // Status
        // =====================================

        public bool IsActive { get; set; }


        // =====================================
        // Inventory
        // =====================================

        public Inventory Inventory { get; set; } = default!;


        // =====================================
        // Images
        // =====================================

        public ICollection<ProductImage> Images { get; set; }
            = new List<ProductImage>();

        // Add this
        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();

        // Orders
        public ICollection<OrderItem> OrderItems { get; set; }
            = new List<OrderItem>();

        // Product Variant
        public ICollection<WishlistItem> WishlistItems { get; set; }
    = new List<WishlistItem>();
    }
}
