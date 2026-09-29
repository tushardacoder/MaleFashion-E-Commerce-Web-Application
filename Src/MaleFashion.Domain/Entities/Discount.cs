using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Discount  : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public string DiscountName { get; set; } = default!;

        public string Code { get; set; } = default!;

        public decimal DiscountPercentage { get; set; }


        public DateTime StartAt { get; set; }

        public DateTime EndAt { get; set; }

        public bool IsActive { get; set; }
    }
}
