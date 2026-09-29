using Cortex.Mediator.Queries;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{
    public class GetProductByIdQuery
        : IQuery<Product?>
    {
        public Guid Id { get; set; }
    }
}
