using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Command
{
    public class AddToCartCommand : ICommand<bool>
    {
        public Guid UserId { get; set; }

        public Guid ProductId { get; set; }

        public Guid ProductVariantId { get; set; }

        public int Quantity { get; set; }
    }
}
