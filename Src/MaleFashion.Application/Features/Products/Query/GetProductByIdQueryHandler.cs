using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Products.Query
{

    public class GetProductByIdQueryHandler
        : IQueryHandler<GetProductByIdQuery, Product?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetProductByIdQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Product?> Handle(
            GetProductByIdQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .ProductRepository
                .GetProductDetailsAsync(
                    query.Id,
                    cancellationToken);
        }
    }
}
