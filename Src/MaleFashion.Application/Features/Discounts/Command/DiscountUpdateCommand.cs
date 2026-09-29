using Cortex.Mediator.Commands;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Command
{

    public class DiscountUpdateCommand : ICommand<Discount>
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
