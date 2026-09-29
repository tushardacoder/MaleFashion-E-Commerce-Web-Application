using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Query
{
    public class GetDiscountByIdQuery
       : IQuery<Discount?>
    {
        public Guid Id { get; set; }
    }
}
