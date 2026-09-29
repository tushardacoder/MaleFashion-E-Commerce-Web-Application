using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{



    public class InventoryRepository
        : Repository<Inventory, Guid>,
          IInventoryRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public InventoryRepository(
            ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        // =====================================================
        // GET PAGED INVENTORIES
        // =====================================================

        public async Task<(
            IList<Inventory> Data,
            int Total,
            int TotalDisplay)>
            GetPagedInventories(
                int pageIndex,
                int pageSize,
                string orderBy,
                string? searchText,
                CancellationToken cancellationToken)
        {
            // ==========================================
            // TOTAL
            // ==========================================

            var totalParameter = new SqlParameter
            {
                ParameterName = "@Total",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };


            // ==========================================
            // TOTAL DISPLAY
            // ==========================================

            var totalDisplayParameter = new SqlParameter
            {
                ParameterName = "@TotalDisplay",
                SqlDbType = SqlDbType.Int,
                Direction = ParameterDirection.Output
            };


            // ==========================================
            // PAGE INDEX
            // ==========================================

            var pageIndexParameter =
                new SqlParameter(
                    "@PageIndex",
                    SqlDbType.Int)
                {
                    Value = pageIndex
                };


            // ==========================================
            // PAGE SIZE
            // ==========================================

            var pageSizeParameter =
                new SqlParameter(
                    "@PageSize",
                    SqlDbType.Int)
                {
                    Value = pageSize
                };


            // ==========================================
            // ORDER BY
            // ==========================================

            var orderByParameter =
                new SqlParameter(
                    "@OrderBy",
                    SqlDbType.NVarChar, 100)
                {
                    Value = string.IsNullOrWhiteSpace(orderBy)
                        ? "Id"
                        : orderBy
                };


            // ==========================================
            // SEARCH TEXT
            // ==========================================

            var searchTextParameter =
                new SqlParameter(
                    "@SearchText",
                    SqlDbType.NVarChar, 250)
                {
                    Value = string.IsNullOrWhiteSpace(searchText)
                        ? DBNull.Value
                        : searchText
                };


            // ==========================================
            // EXECUTE STORED PROCEDURE
            // ==========================================

            var inventories =
                await _dbContext.Inventories
                    .FromSqlRaw(
                        """
                        EXEC [dbo].[GetInventories]
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
                    .ToListAsync(cancellationToken);


            // ==========================================
            // OUTPUT PARAMETERS
            // ==========================================

            var total = Convert.ToInt32(
                totalParameter.Value == DBNull.Value
                    ? 0
                    : totalParameter.Value);


            var totalDisplay = Convert.ToInt32(
                totalDisplayParameter.Value == DBNull.Value
                    ? 0
                    : totalDisplayParameter.Value);


            // ==========================================
            // RETURN
            // ==========================================

            return (
                inventories,
                total,
                totalDisplay);
        }


        // =====================================================
        // GET AVAILABLE PRODUCT VARIANTS
        // =====================================================

        public async Task<IList<ProductVariant>>
            GetAvailableProductVariants(
                CancellationToken cancellationToken)
        {
            return await _dbContext.ProductVariants

                // Load Product information
                .Include(x => x.Product)

                // Only variants without Inventory
                .Where(x =>
                    !_dbContext.Inventories
                        .Any(i =>
                            i.ProductVariantId == x.Id))

                .AsNoTracking()

                .ToListAsync(cancellationToken);
        }

        // =====================================================
        // GET INVENTORY BY ID
        // =====================================================

        // =====================================================
        // GET INVENTORY BY ID
        // =====================================================

        public async Task<Inventory?> GetInventoryByIdAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Inventories
                .Include(x => x.ProductVariant)
                .ThenInclude(x => x.Product)
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }


      public async Task<IList<ProductVariant>>
             GetProductVariantsByIds(
               IList<Guid> ids,
                CancellationToken cancellationToken)
        {
            return await _dbContext.ProductVariants

                .Include(x => x.Product)

                .Where(x => ids.Contains(x.Id))

                .AsNoTracking()

                .ToListAsync(cancellationToken);
        }


        //Order for inventory 
        public async Task<bool> DecreaseStockAsync(
       Guid productVariantId,
       int quantity,
       CancellationToken cancellationToken)
        {
            if (quantity <= 0)
            {
                return false;
            }

            var affectedRows =
                await _dbContext.Inventories
                    .Where(x =>
                        x.ProductVariantId == productVariantId &&
                        x.IsActive &&
                        x.Quantity >= quantity)
                    .ExecuteUpdateAsync(
                        setters => setters
                            .SetProperty(
                                x => x.Quantity,
                                x => x.Quantity - quantity),

                        cancellationToken);

            return affectedRows == 1;
        }
    }
}
