using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Query;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Query
{
    public class GetInventoryByIdQueryHandler
    : IQueryHandler<
        GetInventoryByIdQuery,
        Inventory?>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetInventoryByIdQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<Inventory?> Handle(
       GetInventoryByIdQuery query,
       CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .InventoryRepository
                .GetInventoryByIdAsync(
                    query.Id,
                    cancellationToken);
        }
    }

}

