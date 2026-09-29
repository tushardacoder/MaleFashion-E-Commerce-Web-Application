using MaleFashion.Domain.Contracts;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Domain.Entities
{
    public class ContactUs : IAggregateRoot<Guid>
    {
        public Guid Id { get; set; }


        public string Name { get; set; } = default!;

        public string Email { get; set; }= default!;

        public string Message { get; set; }= default!;

        public DateTime CreatedAt { get; set; }
    }
}
