using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class OrderDetailsViewModel
    {
        // =========================================================
        // ORDER
        // =========================================================

        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }


        // =========================================================
        // CUSTOMER
        // =========================================================

        public string FirstName { get; set; }
            = string.Empty;

        public string LastName { get; set; }
            = string.Empty;

        public string Email { get; set; }
            = string.Empty;

        public string Phone { get; set; }
            = string.Empty;


        // =========================================================
        // SHIPPING
        // =========================================================

        public string Address { get; set; }
            = string.Empty;

        public string TownCity { get; set; }
            = string.Empty;

        public string CountryState { get; set; }
            = string.Empty;

        public string PostcodeZip { get; set; }
            = string.Empty;

        public string? OrderNotes { get; set; }


        // =========================================================
        // PRICE
        // =========================================================

        public decimal Subtotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal Total { get; set; }


        // =========================================================
        // PAYMENT
        // =========================================================

        public string PaymentType { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public decimal PaymentAmount { get; set; }

        public string? MobileNumber { get; set; }

        public string? TransactionId { get; set; }

        public DateTime? PaidAt { get; set; }


        // =========================================================
        // ORDER ITEMS
        // =========================================================

        public List<OrderDetailsItemViewModel> Items { get; set; }
            = new();
    }


    public class OrderDetailsItemViewModel
    {
        public Guid Id { get; set; }

        public Guid ProductVariantId { get; set; }


        // =========================================================
        // PRODUCT SNAPSHOT
        // =========================================================

        public string ProductName { get; set; }
            = string.Empty;

        public string Color { get; set; }
            = string.Empty;

        public string Size { get; set; }
            = string.Empty;

        public string Sku { get; set; }
            = string.Empty;


        // =========================================================
        // PRICE
        // =========================================================

        public decimal UnitPrice { get; set; }


        // =========================================================
        // QUANTITY
        // =========================================================

        public int Quantity { get; set; }


        // =========================================================
        // TOTAL
        // =========================================================

        public decimal TotalPrice { get; set; }
    }
}
