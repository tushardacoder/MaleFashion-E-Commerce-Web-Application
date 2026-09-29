using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Query
{

    public class GetActiveDiscountQueryHandler
        : IQueryHandler<GetActiveDiscountQuery, Discount?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetActiveDiscountQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<Discount?> Handle(
            GetActiveDiscountQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .DiscountRepository
                .GetActiveDiscountAsync(
                    DateTime.Now.AddHours(6),
                    cancellationToken);
        }
    }
}
