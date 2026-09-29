using Cortex.Mediator.Queries;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{

    public class GetShopProductsQuery
         : IQuery<ShopViewModel>
    {
        public string? Search { get; set; }

        public Guid? CategoryId { get; set; }

        public string? Brand { get; set; }

        public string? Size { get; set; }

        public string? Color { get; set; }

        public string? Tag { get; set; }

        public decimal? MinPrice { get; set; }

        public decimal? MaxPrice { get; set; }

        public string? Sort { get; set; }

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 12;
    }
}
