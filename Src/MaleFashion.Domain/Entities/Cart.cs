using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Cart : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
  

        // =====================================
        // Cart Items
        // =====================================

        public ICollection<CartItem> CartItems { get; set; }
            = new List<CartItem>();
    }
}
