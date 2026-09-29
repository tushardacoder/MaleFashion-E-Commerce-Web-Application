using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Carts.Query
{
    public class GetCartQuery : IQuery<CartViewModel>
    {
        public Guid UserId { get; set; }

      
    }
}
