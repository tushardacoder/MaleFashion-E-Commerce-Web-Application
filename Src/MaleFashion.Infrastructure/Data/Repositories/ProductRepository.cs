using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Products.Query;
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
    public class ProductRepository
      : Repository<Product, Guid>,
        IProductRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<ProductRepository> _logger;

        public ProductRepository(
            ApplicationDbContext dbContext,
            ILogger<ProductRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<(IList<Product>, int, int)> GetPagedProducts(
            GetAllProductsByPagingQuery query,
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
                string.IsNullOrWhiteSpace(query.SortText)
                    ? "ProductName ASC"
                    : query.SortText;

            var searchText =
                string.IsNullOrWhiteSpace(query.SearchText)
                    ? (object)DBNull.Value
                    : query.SearchText.Trim();

            _logger.LogInformation(
                "ProductRepository - SearchText: '{SearchText}', PageIndex: {PageIndex}, PageSize: {PageSize}, OrderBy: '{OrderBy}'",
                query.SearchText,
                pageIndex,
                pageSize,
                orderBy);

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
                    100)
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
                    Direction = ParameterDirection.Output
                };

            var totalDisplayParameter =
                new SqlParameter(
                    "@TotalDisplay",
                    SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };

            var products = await _dbContext.Products
                .FromSqlRaw(
                    """
                    EXEC [dbo].[GetProducts]
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

            var total =
                totalParameter.Value == DBNull.Value
                    ? 0
                    : Convert.ToInt32(totalParameter.Value);

            var totalDisplay =
                totalDisplayParameter.Value == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        totalDisplayParameter.Value);

            return (
                products,
                total,
                totalDisplay);
        }

        public async Task<Product?> GetProductDetailsAsync(
            Guid id,
            CancellationToken cancellationToken)
        {
            return await _dbContext.Products
                .Include(x => x.Category)
                .Include(x => x.Variants)
                    .ThenInclude(x => x.Images)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<bool> IsDuplicateProductName(
            string productName,
            Guid? id,
            CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(productName))
                return false;

            productName =
                productName.Trim().ToLower();

            if (!id.HasValue)
            {
                return await GetCountAsync(
                    x =>
                        x.ProductName != null &&
                        x.ProductName.ToLower()
                            == productName,
                    cancellationToken) > 0;
            }

            return await GetCountAsync(
                x =>
                    x.ProductName != null &&
                    x.ProductName.ToLower()
                        == productName &&
                    x.Id != id.Value,
                cancellationToken) > 0;
        }


        public async Task<IList<Product>> GetShopProductsAsync(
    CancellationToken cancellationToken)
        {
            return await _dbContext.Products

                .AsNoTracking()

                // Product → Category
                .Include(x => x.Category)

                // Product → Variants → Images
                .Include(x => x.Variants)
                    .ThenInclude(x => x.Images)

                // Product → Variants → Inventory
                .Include(x => x.Variants)
                    .ThenInclude(x => x.Inventory)

                // Only active products
                .Where(x => x.IsActive)

                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetShopProductDetailsAsync(
    Guid id,
    CancellationToken cancellationToken)
        {
            return await _dbContext.Products

                .Include(x => x.Variants)
                    .ThenInclude(x => x.Images)

                .Include(x => x.Variants)
                    .ThenInclude(x => x.Inventory)

                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

        public async Task<IList<Product>> GetRelatedProductsAsync(
    Guid productId,
    Guid? categoryId,
    CancellationToken cancellationToken)
        {
            var query = _dbContext.Products
                .Where(x =>
                    x.Id != productId &&
                    x.IsActive);

            if (categoryId.HasValue)
            {
                query = query.Where(x =>
                    x.CategoryId == categoryId.Value);
            }

            return await query
                .Include(x => x.Variants)
                    .ThenInclude(x => x.Images)

                .Include(x => x.Variants)
                    .ThenInclude(x => x.Inventory)

                .Take(4)
                .ToListAsync(cancellationToken);
        }

        public async Task<Product?> GetByIdWithVariantsAsync(
          Guid productId,
          CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<Product>()
                .Include(x => x.Variants)
                    .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(
                    x => x.Id == productId,
                    cancellationToken);
        }


        // ==========================================
        // GET PRODUCT VARIANT
        // ==========================================

        public async Task<ProductVariant?> GetVariantByIdAsync(
            Guid variantId,
            CancellationToken cancellationToken)
        {
            return await _dbContext
                .Set<ProductVariant>()
                .Include(x => x.Product)
                .FirstOrDefaultAsync(
                    x => x.Id == variantId,
                    cancellationToken);
        }

        public async Task<ProductVariant?> GetVariantInventoryByIdAsync(
    Guid id,
    CancellationToken cancellationToken)
        {
            return await _dbContext.ProductVariants
                .Include(x => x.Inventory)
                .Include(x => x.Product)
                .FirstOrDefaultAsync(
                    x => x.Id == id,
                    cancellationToken);
        }

       
    }
}


