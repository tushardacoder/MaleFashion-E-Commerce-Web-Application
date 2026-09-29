using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Query
{
    public class GetDiscountByIdQueryHandler
       : IQueryHandler<GetDiscountByIdQuery, Discount?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetDiscountByIdQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Discount?> Handle(
            GetDiscountByIdQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .DiscountRepository
                .GetByIdAsync(
                    query.Id,
                    cancellationToken);
        }
    }
}
