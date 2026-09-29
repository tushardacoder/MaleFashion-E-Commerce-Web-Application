using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class GetShopProductDetailsQuery
        : IQuery<ShopProductDetailsViewModel>
    {
        public Guid Id { get; set; }

        public GetShopProductDetailsQuery(Guid id)
        {
            Id = id;
        }
    }
}
