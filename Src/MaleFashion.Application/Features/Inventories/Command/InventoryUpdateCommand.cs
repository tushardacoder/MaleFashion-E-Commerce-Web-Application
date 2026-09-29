using Cortex.Mediator.Commands;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Command
{
    public class InventoryUpdateCommand : ICommand<Inventory?>
    {
        public Guid Id { get; set; }

        public Guid ProductVariantId { get; set; }

        public int Quantity { get; set; }

        public bool IsActive { get; set; }
    }

}

