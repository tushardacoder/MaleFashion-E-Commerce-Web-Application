using MaleFashion.Application.Features.Orders.Query;
using MaleFashion.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MaleFashion.Application.Contracts.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(
            Order order,
            CancellationToken cancellationToken );

        Task<Order?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken);


        Task<List<Order>> GetByUserIdAsync(
            Guid userId,
            CancellationToken cancellationToken);



        //for admin

        Task<(IList<Order>, int, int)> GetPagedOrders(
           GetAllOrdersByPagingQuery query,
           CancellationToken cancellationToken);

        Task<Order?> GetOrderDetailsAsync(
            Guid orderId,
            CancellationToken cancellationToken);
    }
}
