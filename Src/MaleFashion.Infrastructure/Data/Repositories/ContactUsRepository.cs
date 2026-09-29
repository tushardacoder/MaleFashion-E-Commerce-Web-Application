using MaleFashion.Application.Contracts.Repositories;
using MaleFashion.Application.Features.ContactMessages.Query;
using MaleFashion.Domain.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MaleFashion.Infrastructure.Data.Repositories
{
    public class ContactUsRepository : Repository<ContactUs, Guid>, IContactUsRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ContactUsRepository(ApplicationDbContext dbContext)
            : base(dbContext)
        {
            _dbContext = dbContext;
        }


        public async Task<(IList<ContactUs>, int, int)> GetPagedContactUs(
    GetAllContactUsByPagingQuery query,
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
                ? "CreatedAt DESC"
                : query.SortText;


            // ==========================================
            // SEARCH TEXT
            // ==========================================

            var searchText =
                string.IsNullOrWhiteSpace(query.SearchText)
                    ? (object)DBNull.Value
                    : query.SearchText;


            // ==========================================
            // SQL PARAMETERS
            // ==========================================

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
                    SqlDbType.NVarChar, 50)
                {
                    Value = orderBy
                };


            var searchTextParameter =
                new SqlParameter(
                    "@SearchText",
                    SqlDbType.NVarChar, 250)
                {
                    Value = searchText
                };


            // ==========================================
            // OUTPUT: TOTAL
            // ==========================================

            var totalParameter =
                new SqlParameter(
                    "@Total",
                    SqlDbType.Int)
                {
                    Direction = ParameterDirection.Output
                };


            // ==========================================
            // OUTPUT: TOTAL DISPLAY
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

            var contacts = await _dbContext.ContactUs
                .FromSqlRaw(
                    """
                    EXEC [dbo].[GetContactUs]
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
            // READ OUTPUT PARAMETERS
            // ==========================================

            var total =
                totalParameter.Value == DBNull.Value
                    ? 0
                    : Convert.ToInt32(totalParameter.Value);


            var totalDisplay =
                totalDisplayParameter.Value == DBNull.Value
                    ? 0
                    : Convert.ToInt32(
                        totalDisplayParameter.Value);


            // ==========================================
            // RETURN
            // ==========================================

            return (
                contacts,
                total,
                totalDisplay
            );
        }
    }
}
