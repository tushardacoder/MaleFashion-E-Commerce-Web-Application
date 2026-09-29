using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{

    public class ProductImage 
    {
        public Guid Id { get; set; }


        // =====================================
        // Variant Relationship
        // =====================================

        public Guid ProductVariantId { get; set; }

        public ProductVariant ProductVariant { get; set; }
            = default!;


        // =====================================
        // Image Information
        
        // =====================================

        public string ImageName { get; set; } = default!;


        // =====================================
        // Gallery Ordering
        // =====================================

        public int DisplayOrder { get; set; }


        // =====================================
        // Main Image
        // =====================================
        public bool IsPrimary { get; set; }
    }
}
