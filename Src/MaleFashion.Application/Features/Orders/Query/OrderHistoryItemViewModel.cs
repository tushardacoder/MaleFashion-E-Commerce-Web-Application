using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{

    public class OrderHistoryItemViewModel
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }

        public decimal Subtotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal Total { get; set; }

        public string PaymentType { get; set; }
            = string.Empty;

        public string PaymentStatus { get; set; }
            = string.Empty;

        public int ItemCount { get; set; }
    }


}
