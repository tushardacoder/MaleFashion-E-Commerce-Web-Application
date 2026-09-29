using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Query
{
    public class GetAvailableProductVariantsQuery : IQuery<IList<ProductVariant>> 
    {
    }
}
