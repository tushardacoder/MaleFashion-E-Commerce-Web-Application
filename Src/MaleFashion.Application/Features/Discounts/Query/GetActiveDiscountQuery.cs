using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Query
{
    public class GetActiveDiscountQuery
        : IQuery<Discount?>
    {
    }
}
