using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Inventory : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }


        // =====================================
        // Variant Relationship
        // =====================================

        public Guid ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; }
            = default!;


        // =====================================
        // Stock
        // =====================================

        public int Quantity { get; set; }


        // =====================================
        // Updated
        // =====================================

        public DateTime UpdatedAt { get; set; }


        // =====================================
        // Status
        // =====================================

        public bool IsActive { get; set; }
    }
}
