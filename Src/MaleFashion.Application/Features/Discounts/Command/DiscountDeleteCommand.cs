using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Command
{
    public class DiscountDeleteCommand : ICommand
    {
        public Guid Id { get; set; }
    }
}
