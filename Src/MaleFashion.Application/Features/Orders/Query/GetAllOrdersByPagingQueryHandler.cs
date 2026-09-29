using Cortex.Mediator.Queries;
using MaleFashion.Application.Contracts;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Features.Orders.Query
{
    public class GetAllOrdersByPagingQueryHandler
        : IQueryHandler<
            GetAllOrdersByPagingQuery,
            (IList<Order>, int, int)>
    {
        private readonly IApplicationUnitOfWork
            _applicationUnitOfWork;

        public GetAllOrdersByPagingQueryHandler(
            IApplicationUnitOfWork applicationUnitOfWork)
        {
            _applicationUnitOfWork =
                applicationUnitOfWork;
        }

        public async Task<
            (IList<Order>, int, int)> Handle(
                GetAllOrdersByPagingQuery query,
                CancellationToken cancellationToken)
        {
            return await _applicationUnitOfWork
                .OrderRepository
                .GetPagedOrders(
                    query,
                    cancellationToken);
        }
    }
}
