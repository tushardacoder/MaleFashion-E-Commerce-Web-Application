using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class Category : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }

        public string CategoryName { get; set; } = default!;

        public bool IsActive { get; set; }
        public ICollection<Product> Products { get; set; }
            = new List<Product>();

    }
}
