using MaleFashion.Domain.Contracts;
using MaleFashion.Domain.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Order : IAggregateRoot<Guid>
    {
       
            public Guid Id { get; set; }

            public Guid UserId { get; set; }

            public string FirstName { get; set; } = default!;

            public string LastName { get; set; } = default!;

            public string Email { get; set; } = default!;

            public string Phone { get; set; } = default!;

            public string Address { get; set; } = default!;

            public string TownCity { get; set; } = default!;

            public string CountryState { get; set; } = default!;

            public string PostcodeZip { get; set; } = default!;

            public string? OrderNotes { get; set; }
            public decimal Subtotal { get; set; }

           public decimal DiscountAmount { get; set; }
           public decimal Total { get; set; }

           public Payment? Payment { get; set; }
           public DateTime CreatedAt { get; set; }
           public ICollection<OrderItem> OrderItems { get; set; }
                = new List<OrderItem>();
        }
}
