using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Command
{
    public class RemoveFromCartCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }

        public Guid ProductVariantId { get; set; }
    }
}



