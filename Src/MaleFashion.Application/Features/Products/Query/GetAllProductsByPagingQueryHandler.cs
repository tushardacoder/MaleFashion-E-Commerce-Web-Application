using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{

    public class GetAllProductsByPagingQueryHandler
        : IQueryHandler<
            GetAllProductsByPagingQuery,
            (IList<Product>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAllProductsByPagingQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<Product>, int, int)> Handle(
            GetAllProductsByPagingQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .ProductRepository
                .GetPagedProducts(
                    query,
                    cancellationToken);
        }
    }
}
