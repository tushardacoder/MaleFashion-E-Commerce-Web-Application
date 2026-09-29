using Cortex.Mediator.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Command
{
    public class InventoryDeleteCommand : ICommand<bool> 
    { 
        public Guid Id { get; set; } 
    }
}
