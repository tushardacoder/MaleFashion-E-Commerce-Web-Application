using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class OrderDetailsAdminViewModel
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }


        // CUSTOMER

        public string FirstName { get; set; } = default!;

        public string LastName { get; set; } = default!;

        public string Email { get; set; } = default!;

        public string Phone { get; set; } = default!;


        // SHIPPING

        public string Address { get; set; } = default!;

        public string TownCity { get; set; } = default!;

        public string CountryState { get; set; } = default!;

        public string PostcodeZip { get; set; } = default!;

        public string? OrderNotes { get; set; }


        // PRICE

        public decimal Subtotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal Total { get; set; }


        // DATE

        public DateTime CreatedAt { get; set; }


        // ITEMS

        public IList<OrderItemDetailsAdminViewModel>
            OrderItems
        { get; set; }
            = new List<OrderItemDetailsAdminViewModel>();


        // PAYMENT

        public PaymentDetailsAdminViewModel? Payment { get; set; }
    }


    public class OrderItemDetailsAdminViewModel
    {
        public Guid Id { get; set; }

        public Guid ProductVariantId { get; set; }

        public string ProductName { get; set; } = default!;

        public string Color { get; set; } = default!;

        public string Size { get; set; } = default!;

        public string Sku { get; set; } = default!;

        public decimal UnitPrice { get; set; }

        public int Quantity { get; set; }

        public decimal TotalPrice { get; set; }
    }

    public class PaymentDetailsAdminViewModel
    {
        public Guid Id { get; set; }

        public PaymentType PaymentType { get; set; }

        public decimal Amount { get; set; }

        public string? MobileNumber { get; set; }

        public string? TransactionId { get; set; }

        public PaymentStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? PaidAt { get; set; }
    }
}
