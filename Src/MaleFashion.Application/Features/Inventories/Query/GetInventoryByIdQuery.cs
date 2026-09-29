using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Query
{
    public class GetInventoryByIdQuery
      : IQuery<Inventory?>
    {
        public Guid Id { get; set; }
    }
}

