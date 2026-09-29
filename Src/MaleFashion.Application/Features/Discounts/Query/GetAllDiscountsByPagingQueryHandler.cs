using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Discounts.Query
{
    public class GetAllDiscountsByPagingQueryHandler
        : IQueryHandler<
            GetAllDiscountsByPagingQuery,
            (IList<Discount>, int, int)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAllDiscountsByPagingQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<(IList<Discount>, int, int)> Handle(
            GetAllDiscountsByPagingQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .DiscountRepository
                .GetPagedDiscounts(
                    query,
                    cancellationToken);
        }
    }
}
