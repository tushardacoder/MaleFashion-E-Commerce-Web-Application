using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class GetOrderDetailsAdminQuery
       : IQuery<OrderDetailsAdminViewModel?>
    {
        public Guid OrderId { get; set; }
    }
}
