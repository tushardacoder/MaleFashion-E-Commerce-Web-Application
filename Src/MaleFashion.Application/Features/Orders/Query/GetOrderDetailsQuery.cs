using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class GetOrderDetailsQuery
       : IQuery<OrderDetailsViewModel>
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }
    }
}
