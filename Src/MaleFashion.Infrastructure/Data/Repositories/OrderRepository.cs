using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Orders.Query;
using MaleFashion.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(
            ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(
            Order order,
            CancellationToken cancellationToken)
        {
            await _context.Orders.AddAsync(
                order,
                cancellationToken);
        }

        public async Task<Order?> GetByIdAsync(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            return await _context.Orders
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);
        }


        // ========================================================= // GET ALL ORDERS FOR USER // =========================================================
        public async Task<List<Order>> GetByUserIdAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Orders.AsNoTracking()
                .Include(x => x.OrderItems)
                .Include(x => x.Payment)
                .Where(x => x.UserId == userId)
                .OrderByDescending(x => x.CreatedAt).ToListAsync(cancellationToken); 
       }



    //admin

      // GET PAGED ORDERS
        // ==========================================

        public async Task<
            (IList<Order>, int, int)> GetPagedOrders(
                GetAllOrdersByPagingQuery query,
                CancellationToken cancellationToken)
        {
            var pageIndex =
                query.PageIndex <= 0
                    ? 1
                    : query.PageIndex;


            var pageSize =
                query.PageSize <= 0
                    ? 10
                    : query.PageSize;


            var orderBy =
                string.IsNullOrWhiteSpace(
                    query.SortText)
                    ? "CreatedAt DESC"
                    : query.SortText;


            var searchText =
                string.IsNullOrWhiteSpace(
                    query.SearchText)
                    ? (object)DBNull.Value
                    : query.SearchText;


            var pageIndexParameter =
                new SqlParameter(
                    "@PageIndex",
                    SqlDbType.Int)
                {
                    Value = pageIndex
                };


            var pageSizeParameter =
                new SqlParameter(
                    "@PageSize",
                    SqlDbType.Int)
                {
                    Value = pageSize
                };


            var orderByParameter =
                new SqlParameter(
                    "@OrderBy",
                    SqlDbType.NVarChar,
                    50)
                {
                    Value = orderBy
                };


            var searchTextParameter =
                new SqlParameter(
                    "@SearchText",
                    SqlDbType.NVarChar,
                    250)
                {
                    Value = searchText
                };


            var totalParameter =
                new SqlParameter(
                    "@Total",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var totalDisplayParameter =
                new SqlParameter(
                    "@TotalDisplay",
                    SqlDbType.Int)
                {
                    Direction =
                        ParameterDirection.Output
                };


            var orders =
                await _context.Orders
                    .FromSqlRaw(
                        """
                        EXEC [dbo].[GetOrders]
                            @PageIndex,
                            @PageSize,
                            @OrderBy,
                            @SearchText,
                            @Total OUTPUT,
                            @TotalDisplay OUTPUT
                        """,
                        pageIndexParameter,
                        pageSizeParameter,
                        orderByParameter,
                        searchTextParameter,
                        totalParameter,
                        totalDisplayParameter)
                    .AsNoTracking()
                    .ToListAsync(
                        cancellationToken);


            var total =
                totalParameter.Value ==
                    DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        totalParameter.Value);


            var totalDisplay =
                totalDisplayParameter.Value ==
                    DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        totalDisplayParameter.Value);


            return (
                orders,
                total,
                totalDisplay);
        }


        // ==========================================
        // GET ORDER DETAILS
        // ==========================================

        public async Task<Order?> GetOrderDetailsAsync(
            Guid orderId,
            CancellationToken cancellationToken)
        {
            return await _context.Orders

                .Include(x =>
                    x.OrderItems)

                .Include(x =>
                    x.Payment)

                .AsNoTracking()

                .FirstOrDefaultAsync(
                    x => x.Id == orderId,
                    cancellationToken);
        }
    }
}
