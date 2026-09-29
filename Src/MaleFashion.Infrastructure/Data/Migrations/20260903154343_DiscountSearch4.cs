using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DiscountSearch4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetDiscounts]
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


                    --------------------------------------------------
                    -- TOTAL RECORDS
                    --------------------------------------------------

                    SELECT @Total = COUNT(*)
                    FROM dbo.Discount;


                    --------------------------------------------------
                    -- SAFE DEFAULT SORT
                    --------------------------------------------------

                    IF @OrderBy NOT IN
                    (
                        'DiscountName ASC',
                        'DiscountName DESC',

                        'Code ASC',
                        'Code DESC',

                        'DiscountAmount ASC',
                        'DiscountAmount DESC',

                        'StartAt ASC',
                        'StartAt DESC',

                        'EndAt ASC',
                        'EndAt DESC',

                        'IsActive ASC',
                        'IsActive DESC'
                    )
                    BEGIN
                        SET @OrderBy = 'DiscountName ASC';
                    END;


                    --------------------------------------------------
                    -- FILTERED COUNT
                    --------------------------------------------------

                    SET @countSql = '
                        SELECT @xTotalDisplay = COUNT(*)
                        FROM dbo.Discount
                        WHERE 1 = 1
                    ';


                    --------------------------------------------------
                    -- SEARCH DISCOUNT NAME OR CODE
                    --------------------------------------------------

                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN
                        SET @countSql += '
                            AND
                            (
                                DiscountName LIKE ''%'' + @xSearchText + ''%''
                                OR
                                Code LIKE ''%'' + @xSearchText + ''%''
                            )
                        ';
                    END;


                    --------------------------------------------------
                    -- COUNT PARAMETERS
                    --------------------------------------------------

                    SET @countParamList = '
                        @xSearchText nvarchar(250),
                        @xTotalDisplay int OUTPUT
                    ';


                    --------------------------------------------------
                    -- EXECUTE FILTERED COUNT
                    --------------------------------------------------

                    EXEC sp_executesql
                        @countSql,
                        @countParamList,
                        @xSearchText = @SearchText,
                        @xTotalDisplay = @TotalDisplay OUTPUT;


                    --------------------------------------------------
                    -- MAIN QUERY
                    --------------------------------------------------

                    SET @sql = '
                        SELECT *
                        FROM dbo.Discount
                        WHERE 1 = 1
                    ';


                    --------------------------------------------------
                    -- SEARCH DISCOUNT NAME OR CODE
                    --------------------------------------------------

                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN
                        SET @sql += '
                            AND
                            (
                                DiscountName LIKE ''%'' + @xSearchText + ''%''
                                OR
                                Code LIKE ''%'' + @xSearchText + ''%''
                            )
                        ';
                    END;


                    --------------------------------------------------
                    -- ORDER + PAGING
                    --------------------------------------------------

                    SET @sql += '
                        ORDER BY ' + @OrderBy + '

                        OFFSET @xPageSize * (@xPageIndex - 1) ROWS

                        FETCH NEXT @xPageSize ROWS ONLY
                    ';


                    --------------------------------------------------
                    -- MAIN PARAMETERS
                    --------------------------------------------------

                    SET @paramList = '
                        @xSearchText nvarchar(250),
                        @xPageIndex int,
                        @xPageSize int
                    ';


                    --------------------------------------------------
                    -- EXECUTE MAIN QUERY
                    --------------------------------------------------

                    EXEC sp_executesql
                        @sql,
                        @paramList,
                        @xSearchText = @SearchText,
                        @xPageIndex = @PageIndex,
                        @xPageSize = @PageSize;

                END
                """;

            migrationBuilder.Sql(sql);

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                DROP PROCEDURE IF EXISTS [dbo].[GetDiscounts]
                """);
        }
    }
}
