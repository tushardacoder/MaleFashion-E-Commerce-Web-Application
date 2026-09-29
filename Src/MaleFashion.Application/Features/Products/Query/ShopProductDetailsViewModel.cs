using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class ShopProductDetailsViewModel
    {
        public Guid Id { get; set; }

        public string ProductName { get; set; }
            = string.Empty;

        public string Branding { get; set; }
            = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; }
            = string.Empty;

        public string CustomerPreview { get; set; } = string.Empty;
        public IList<ShopProductViewModel> RelatedProducts { get; set; }
    = new List<ShopProductViewModel>();

        public string? Sku { get; set; }

        public string? CategoryName { get; set; }

        public string? PrimaryImage { get; set; }

        public IList<string> Images { get; set; }
            = new List<string>();

        public IList<string> Colors { get; set; }
            = new List<string>();

        public IList<string> Sizes { get; set; }
            = new List<string>();

        public IList<string> Tags { get; set; }
            = new List<string>();

        public bool IsInStock { get; set; }

        //ProductVariant
        public IList<ShopProductVariantViewModel> Variants { get; set; } = new List<ShopProductVariantViewModel>();
        public bool IsActive { get; set; }
    }

    
public class ShopProductVariantViewModel
    {
        public Guid Id { get; set; }

        public string? Color { get; set; }

        public string? Size { get; set; }

        public bool IsInStock { get; set; }

        public int InventoryQuantity { get; set; }
    }


}
