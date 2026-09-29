using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Payment
    {
       

        public Guid Id { get; set; }


   

        public Guid OrderId { get; set; }

        public Order Order { get; set; } = default!;


    
        public PaymentType PaymentType { get; set; }



        public decimal Amount { get; set; }


        public string? MobileNumber { get; set; }
        public string? TransactionId { get; set; }



        public PaymentStatus Status { get; set; }


        public DateTime CreatedAt { get; set; }
        public DateTime? PaidAt { get; set; }
    }
}
