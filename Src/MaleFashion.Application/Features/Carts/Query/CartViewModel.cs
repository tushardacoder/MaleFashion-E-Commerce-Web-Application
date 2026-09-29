using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Query
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; }
           = new();

        public decimal Subtotal { get; set; }

        public decimal Discount { get; set; }
        public string? CouponCode { get; set; }

        public string? CouponName { get; set;  }
        public decimal Total { get; set; }
    }

    public class CartItemViewModel
    {
        public Guid ProductVariantId { get; set; }

        public string ProductName { get; set; }

        public string ImageName { get; set; }

        public string Branding { get; set; }

        public string Size { get; set; }

        public string Color { get; set; }

        public string Sku { get; set; }

        public decimal UnitPrice { get; set; }


        // Available inventory
        public int Stock { get; set; }
        public int Quantity { get; set; }

        public decimal Total =>
            UnitPrice * Quantity;
    }
}


