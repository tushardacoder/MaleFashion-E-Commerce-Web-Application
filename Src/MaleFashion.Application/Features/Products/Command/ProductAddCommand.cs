using Cortex.Mediator.Commands;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Command
{
    public class ProductAddCommand
        : ICommand<Product>
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; } = default!;

        public string Branding { get; set; } = default!;

        public decimal ProductPrize { get; set; }

        public List<string> Tags { get; set; }
            = new();

        public string Description { get; set; } = default!;

        public string CustomerPreview { get; set; } = default!;

        public string AdditionalInfo { get; set; } = default!;

        public bool IsActive { get; set; }

        public Guid CategoryId { get; set; }

        public List<ProductVariantCommand> Variants { get; set; }
            = new();
    }

    public class ProductVariantCommand
    {
        public Guid Id { get; set; }

        public string Sku { get; set; } = default!;

        public string Size { get; set; } = default!;

        public string Color { get; set; } = default!;

        public bool IsActive { get; set; }

        public List<ProductImageCommand> Images { get; set; }
            = new();
    }

    public class ProductImageCommand
    {
        public Guid Id { get; set; }

        public string ImageName { get; set; } = default!;

        public int DisplayOrder { get; set; }

        public bool IsPrimary { get; set; }
    }
}
