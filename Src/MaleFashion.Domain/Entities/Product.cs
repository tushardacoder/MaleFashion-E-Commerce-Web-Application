using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Product : IAggregateRoot<Guid>
    {

        public Guid Id { get; set; }
        // =====================================
        // Product Information
        // =====================================

        public string ProductName { get; set; } = default!;

        public string Branding { get; set; } = default!;

        public decimal ProductPrize { get; set; }


        public List<string> Tags { get; set; } = new();


        public string Description { get; set; } = default!;

        public string CustomerPreview { get; set; } = default!;

        public string AdditionalInfo { get; set; } = default!;

        public bool IsActive { get; set; }

        // =====================================
        // Category
        // =====================================

        public Guid CategoryId { get; set; }

        public Category Category { get; set; } = default!;


        // =====================================
        // Variants
        // =====================================

        public ICollection<ProductVariant> Variants { get; set; }
            = new List<ProductVariant>();



      



    }
}
