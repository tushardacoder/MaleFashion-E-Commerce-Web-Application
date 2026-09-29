using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Wishlists.Query
{
    public class WishlistItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public Guid ProductVariantId { get; set; }

        public string ProductName { get; set; }
            = string.Empty;

        public string Branding { get; set; }
            = string.Empty;

        public string Color { get; set; }
            = string.Empty;

        public string Size { get; set; }
            = string.Empty;

        public string Sku { get; set; }
            = string.Empty;

        public decimal Price { get; set; }

        public string? Image { get; set; }

        public DateTime AddedAt { get; set; }

        public bool IsInStock { get; set; }
    }
}
