using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class GetAllProductsByPagingQuery
        : IQuery<(IList<Product>, int, int)>
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string SearchText { get; set; }
            = string.Empty;

        public string SortText { get; set; }
            = string.Empty;
    }
}
