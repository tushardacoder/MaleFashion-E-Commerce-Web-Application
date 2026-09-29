using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Query;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Query
{
    public class GetAvailableProductVariantsQueryHandler
       : IQueryHandler<
           GetAvailableProductVariantsQuery,
           IList<ProductVariant>>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAvailableProductVariantsQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<IList<ProductVariant>> Handle(
            GetAvailableProductVariantsQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .InventoryRepository
                .GetAvailableProductVariants(
                    cancellationToken);
        }
    }
}

