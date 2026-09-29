using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class ContactusSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetContactUs]
                    @PageIndex int,
                    @PageSize int,
                    @OrderBy nvarchar(50),
                    @SearchText nvarchar(250) = NULL,
                    @Total int OUTPUT,
                    @TotalDisplay int OUTPUT
                AS
                BEGIN

                    SET NOCOUNT ON;

                    DECLARE @sql nvarchar(MAX);
                    DECLARE @countSql nvarchar(MAX);

                    DECLARE @paramList nvarchar(MAX);
                    DECLARE @countParamList nvarchar(MAX);


                    -- ==========================================
                    -- TOTAL RECORDS
                    -- ==========================================

                    SELECT @Total = COUNT(*)
                    FROM ContactUs;


                    -- ==========================================
                    -- FILTERED COUNT
                    -- ==========================================

                    SET @countSql = '
                        SELECT @xTotalDisplay = COUNT(*)
                        FROM ContactUs
                        WHERE 1 = 1
                    ';


                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @countSql = @countSql + '
                            AND
                            (
                                Name LIKE ''%'' + @xSearchText + ''%''
                                OR Email LIKE ''%'' + @xSearchText + ''%''
                                OR Message LIKE ''%'' + @xSearchText + ''%''
                            )
                        ';

                    END;


                    -- ==========================================
                    -- MAIN QUERY
                    -- ==========================================

                    SET @sql = '
                        SELECT *
                        FROM ContactUs
                        WHERE 1 = 1
                    ';


                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @sql = @sql + '
                            AND
                            (
                                Name LIKE ''%'' + @xSearchText + ''%''
                                OR Email LIKE ''%'' + @xSearchText + ''%''
                                OR Message LIKE ''%'' + @xSearchText + ''%''
                            )
                        ';

                    END;


                    -- ==========================================
                    -- SORTING + PAGING
                    -- ==========================================

                    SET @sql = @sql + '
                        ORDER BY ' + @OrderBy + '
                        OFFSET @xPageSize * (@xPageIndex - 1) ROWS
                        FETCH NEXT @xPageSize ROWS ONLY
                    ';


                    -- ==========================================
                    -- COUNT PARAMETERS
                    -- ==========================================

                    SET @countParamList = '
                        @xSearchText nvarchar(250),
                        @xTotalDisplay int OUTPUT
                    ';


                    EXEC sp_executesql
                        @countSql,
                        @countParamList,
                        @SearchText,
                        @xTotalDisplay = @TotalDisplay OUTPUT;


                    -- ==========================================
                    -- MAIN QUERY PARAMETERS
                    -- ==========================================

                    SET @paramList = '
                        @xSearchText nvarchar(250),
                        @xPageIndex int,
                        @xPageSize int
                    ';


                    EXEC sp_executesql
                        @sql,
                        @paramList,
                        @SearchText,
                        @PageIndex,
                        @PageSize;

                END
                """;

            migrationBuilder.Sql(sql);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP PROCEDURE IF EXISTS [dbo].[GetContactUs]
                """);
        }
    }
}
