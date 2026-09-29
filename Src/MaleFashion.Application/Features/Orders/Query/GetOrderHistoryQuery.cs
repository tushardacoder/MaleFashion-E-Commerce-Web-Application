using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{

    public class GetOrderHistoryQuery
        : IQuery<OrderHistoryViewModel>
    {
        public Guid UserId { get; set; }
    }
}
