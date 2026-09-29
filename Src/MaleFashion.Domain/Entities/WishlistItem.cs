using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class WishlistItem : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }


        // =====================================
        // Wishlist
        // =====================================

        public Guid WishlistId { get; set; }

        public WishList Wishlist { get; set; } = default!;


        // =====================================
        // Product Variant
        // =====================================

        public Guid ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; }
            = default!;


        // =====================================
        // Added Date
        // =====================================

        public DateTime AddedAt { get; set; }
    }
}
