using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Discounts.Query;
using MaleFashion.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{
    public class DiscountRepository
        : Repository<Discount, Guid>,
          IDiscountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<DiscountRepository> _logger;


        public DiscountRepository(
            ApplicationDbContext dbContext,
            ILogger<DiscountRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }


        // =========================================================
        // GET PAGED DISCOUNTS
        // =========================================================
        public async Task<(IList<Discount>, int, int)> GetPagedDiscounts(
    GetAllDiscountsByPagingQuery query,
    CancellationToken cancellationToken)
        { 
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


            var searchTextParameter =
                new SqlParameter(
                    "@SearchText",
                    SqlDbType.NVarChar,
                    250)
                {
                    Value =
                        string.IsNullOrWhiteSpace(
                            query.SearchText)
                            ? DBNull.Value
                            : query.SearchText.Trim()
                };


            var orderBy =
                string.IsNullOrWhiteSpace(
                    query.SortText)
                    ? "DiscountName ASC"
                    : query.SortText;


            var pageIndexParameter =
                new SqlParameter(
                    "@PageIndex",
                    SqlDbType.Int)
                {
                    Value = query.PageIndex
                };


            var pageSizeParameter =
                new SqlParameter(
                    "@PageSize",
                    SqlDbType.Int)
                {
                    Value = query.PageSize
                };


            var orderByParameter =
                new SqlParameter(
                    "@OrderBy",
                    SqlDbType.NVarChar,
                    50)
                {
                    Value = orderBy
                };


            var parameters = new[]
            {
        pageIndexParameter,

        pageSizeParameter,

        orderByParameter,

        searchTextParameter,

        totalParameter,

        totalDisplayParameter
    };


            var data =
                await _dbContext
                    .Discount
                    .FromSqlRaw(
                        """
                EXEC dbo.GetDiscounts
                    @PageIndex,
                    @PageSize,
                    @OrderBy,
                    @SearchText,
                    @Total OUTPUT,
                    @TotalDisplay OUTPUT
                """,
                        parameters)
                    .AsNoTracking()
                    .ToListAsync(
                        cancellationToken);


            var total =
                totalParameter.Value == DBNull.Value
                    ? 0
                    : (int)totalParameter.Value;


            var totalDisplay =
                totalDisplayParameter.Value == DBNull.Value
                    ? 0
                    : (int)totalDisplayParameter.Value;


            return (
                data,
                total,
                totalDisplay);
        }

        // =========================================================
        // DUPLICATE CODE
        // =========================================================

        public async Task<bool> IsDuplicateDiscountCode(
            string code,
            Guid? id,
            CancellationToken cancellationToken)
        {
            // ==========================================
            // VALIDATE
            // ==========================================

            if (string.IsNullOrWhiteSpace(code))
            {
                return false;
            }


            // ==========================================
            // NORMALIZE
            // ==========================================

            code =
                code.Trim()
                    .ToLower();


            // ==========================================
            // CREATE
            // ==========================================

            if (!id.HasValue)
            {
                return await GetCountAsync(
                    x =>
                        x.Code != null &&
                        x.Code.ToLower() == code,
                    cancellationToken) > 0;
            }


            // ==========================================
            // UPDATE
            // ==========================================

            return await GetCountAsync(
                x =>
                    x.Code != null &&
                    x.Code.ToLower() == code &&
                    x.Id != id.Value,
                cancellationToken) > 0;
        }

        public async Task<Discount?> GetActiveDiscountAsync(
    DateTime currentTime,
    CancellationToken cancellationToken)
        {
            return await _dbContext.Discount
                .Where(x =>
                    x.IsActive &&
                    x.StartAt <= currentTime &&
                    x.EndAt >= currentTime)
                .OrderByDescending(x => x.DiscountPercentage)
                .FirstOrDefaultAsync(cancellationToken);
        }


    }
}
