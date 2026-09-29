using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.Categories.Query;
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
     public class CategoryRepository
            : Repository<Category, Guid>,
              ICategoryRepository
        {
        private readonly ApplicationDbContext _dbContext;
        private readonly ILogger<CategoryRepository> _logger;

        public CategoryRepository(
            ApplicationDbContext dbContext,
            ILogger<CategoryRepository> logger)
            : base(dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<(IList<Category>, int, int)> GetPagedCategories(
                GetAllCategoriesByPagingQuery query,
                CancellationToken cancellationToken)
            {

            
            // ==========================================
            // PAGE INDEX
            // ==========================================

            var pageIndex = query.PageIndex <= 0
                    ? 1
                    : query.PageIndex;


                // ==========================================
                // PAGE SIZE
                // ==========================================

                var pageSize = query.PageSize <= 0
                    ? 10
                    : query.PageSize;


                // ==========================================
                // ORDER BY
                // ==========================================

                var orderBy = string.IsNullOrWhiteSpace(query.SortText)
                    ? "CategoryName ASC"
                    : query.SortText;


                // ==========================================
                // SEARCH TEXT
                // ==========================================

                var searchText =
                    string.IsNullOrWhiteSpace(query.SearchText)
                        ? (object)DBNull.Value
                        : query.SearchText;


            // ==========================================
            // LOG BEFORE SQL
            // ==========================================

            _logger.LogInformation(
                "CategoryRepository - SearchText: '{SearchText}', PageIndex: {PageIndex}, PageSize: {PageSize}, OrderBy: '{OrderBy}'",
                query.SearchText,
                pageIndex,
                pageSize,
                orderBy);



            // ==========================================
            // SQL PARAMETER: PAGE INDEX
            // ==========================================

            var pageIndexParameter =
                    new SqlParameter(
                        "@PageIndex",
                        SqlDbType.Int)
                    {
                        Value = pageIndex
                    };


                // ==========================================
                // SQL PARAMETER: PAGE SIZE
                // ==========================================

                var pageSizeParameter =
                    new SqlParameter(
                        "@PageSize",
                        SqlDbType.Int)
                    {
                        Value = pageSize
                    };


                // ==========================================
                // SQL PARAMETER: ORDER BY
                // ==========================================

                var orderByParameter =
                    new SqlParameter(
                        "@OrderBy",
                        SqlDbType.NVarChar, 50)
                    {
                        Value = orderBy
                    };


                // ==========================================
                // SQL PARAMETER: SEARCH TEXT
                // ==========================================

                var searchTextParameter =
                    new SqlParameter(
                        "@SearchText",
                        SqlDbType.NVarChar, 250)
                    {
                        Value = searchText
                    };


                // ==========================================
                // OUTPUT PARAMETER: TOTAL
                // ==========================================

                var totalParameter =
                    new SqlParameter(
                        "@Total",
                        SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };


                // ==========================================
                // OUTPUT PARAMETER: TOTAL DISPLAY
                // ==========================================

                var totalDisplayParameter =
                    new SqlParameter(
                        "@TotalDisplay",
                        SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };


                // ==========================================
                // EXECUTE STORED PROCEDURE
                // ==========================================

                var categories = await _dbContext.Categories
                    .FromSqlRaw(
                        """
                    EXEC [dbo].[GetCategories]
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
                // READ TOTAL
                // ==========================================

                var total =
                    totalParameter.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            totalParameter.Value);


                // ==========================================
                // READ TOTAL DISPLAY
                // ==========================================

                var totalDisplay =
                    totalDisplayParameter.Value == DBNull.Value
                        ? 0
                        : Convert.ToInt32(
                            totalDisplayParameter.Value);


            // ==========================================
            // LOG AFTER SQL
            // ==========================================

            _logger.LogInformation(
                "CategoryRepository - SearchText: '{SearchText}', Categories Returned: {Count}, Total: {Total}, TotalDisplay: {TotalDisplay}",
                query.SearchText,
                categories.Count,
                total,
                totalDisplay);

            // ==========================================
            // RETURN
            // ==========================================

            return (
                    categories,
                    total,
                    totalDisplay
                );
            }


            public async Task<bool> IsDuplicateCategoryName(
                string categoryName,
                Guid? id,
                CancellationToken cancellationToken)
            {
                // ==========================================
                // VALIDATE CATEGORY NAME
                // ==========================================

                if (string.IsNullOrWhiteSpace(categoryName))
                {
                    return false;
                }


                // ==========================================
                // NORMALIZE CATEGORY NAME
                // ==========================================

                categoryName = categoryName
                    .Trim()
                    .ToLower();


                // ==========================================
                // CREATE
                // ==========================================

                if (!id.HasValue)
                {
                    return await GetCountAsync(
                        x =>
                            x.CategoryName != null &&
                            x.CategoryName.ToLower() == categoryName,
                        cancellationToken) > 0;
                }


                // ==========================================
                // UPDATE
                // ==========================================

                return await GetCountAsync(
                    x =>
                        x.CategoryName != null &&
                        x.CategoryName.ToLower() == categoryName &&
                        x.Id != id.Value,
                    cancellationToken) > 0;
            }
        }
    }






