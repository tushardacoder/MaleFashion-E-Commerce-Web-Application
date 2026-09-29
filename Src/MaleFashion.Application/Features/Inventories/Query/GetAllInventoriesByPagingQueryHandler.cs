using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Application.Features.Inventories.Query;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Inventories.Query
{
    public class GetAllInventoriesByPagingQueryHandler
      : IQueryHandler<
          GetAllInventoriesByPagingQuery,
          (IList<Inventory> Data,
           int Total,
           int TotalDisplay)>
    {
        private readonly IApplicationUnitOfWork _unitOfWork;

        public GetAllInventoriesByPagingQueryHandler(
            IApplicationUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        public async Task<(
            IList<Inventory> Data,
            int Total,
            int TotalDisplay)> Handle(
            GetAllInventoriesByPagingQuery query,
            CancellationToken cancellationToken)
        {
            return await _unitOfWork
                .InventoryRepository
                .GetPagedInventories(
                    query.PageIndex,
                    query.PageSize,
                    query.SortText,
                    query.SearchText,
                    cancellationToken);
        }
    }
}

