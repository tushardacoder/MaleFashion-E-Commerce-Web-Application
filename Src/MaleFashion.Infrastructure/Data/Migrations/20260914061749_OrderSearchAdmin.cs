using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class OrderSearchAdmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetOrders]
                    @PageIndex int,
                    @PageSize int,
                    @OrderBy nvarchar(50),
                    @SearchText nvarchar(250) = NULL,
                    @Total int OUTPUT,
                    @TotalDisplay int OUTPUT
                AS
                BEGIN

                    SET NOCOUNT ON;


                    -- ==========================================
                    -- 1. TOTAL ORDERS
                    -- ==========================================

                    SELECT @Total = COUNT(*)
                    FROM Orders;


                    -- ==========================================
                    -- 2. VALIDATE SORTING
                    -- ==========================================

                    SET @OrderBy =
                        CASE @OrderBy

                            WHEN 'FirstName ASC'
                                THEN 'FirstName ASC'

                            WHEN 'FirstName DESC'
                                THEN 'FirstName DESC'

                            WHEN 'LastName ASC'
                                THEN 'LastName ASC'

                            WHEN 'LastName DESC'
                                THEN 'LastName DESC'

                            WHEN 'Email ASC'
                                THEN 'Email ASC'

                            WHEN 'Email DESC'
                                THEN 'Email DESC'

                            WHEN 'Phone ASC'
                                THEN 'Phone ASC'

                            WHEN 'Phone DESC'
                                THEN 'Phone DESC'

                            WHEN 'Subtotal ASC'
                                THEN 'Subtotal ASC'

                            WHEN 'Subtotal DESC'
                                THEN 'Subtotal DESC'

                            WHEN 'DiscountAmount ASC'
                                THEN 'DiscountAmount ASC'

                            WHEN 'DiscountAmount DESC'
                                THEN 'DiscountAmount DESC'

                            WHEN 'Total ASC'
                                THEN 'Total ASC'

                            WHEN 'Total DESC'
                                THEN 'Total DESC'

                            WHEN 'CreatedAt ASC'
                                THEN 'CreatedAt ASC'

                            WHEN 'CreatedAt DESC'
                                THEN 'CreatedAt DESC'

                            ELSE 'CreatedAt DESC'

                        END;


                    -- ==========================================
                    -- 3. FILTERED COUNT
                    -- ==========================================

                    DECLARE @countSql nvarchar(MAX);


                    SET @countSql = '
                        SELECT @xTotalDisplay = COUNT(*)
                        FROM Orders
                        WHERE 1 = 1
                    ';


                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @countSql = @countSql + '
                            AND
                            (
                                FirstName LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR LastName LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR Email LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR Phone LIKE ''%'' +
                                    @xSearchText + ''%''
                            )
                        ';

                    END;


                    -- ==========================================
                    -- 4. MAIN QUERY
                    -- ==========================================

                    DECLARE @sql nvarchar(MAX);


                    SET @sql = '
                        SELECT *
                        FROM Orders
                        WHERE 1 = 1
                    ';


                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @sql = @sql + '
                            AND
                            (
                                FirstName LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR LastName LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR Email LIKE ''%'' +
                                    @xSearchText + ''%''

                                OR Phone LIKE ''%'' +
                                    @xSearchText + ''%''
                            )
                        ';

                    END;


                    -- ==========================================
                    -- 5. PAGING
                    -- ==========================================

                    SET @sql = @sql + '
                        ORDER BY ' + @OrderBy + '

                        OFFSET
                            @xPageSize *
                            (@xPageIndex - 1)
                            ROWS

                        FETCH NEXT
                            @xPageSize
                            ROWS ONLY
                    ';


                    -- ==========================================
                    -- 6. FILTERED COUNT EXECUTION
                    -- ==========================================

                    DECLARE @countParamList nvarchar(MAX);


                    SET @countParamList = '
                        @xSearchText nvarchar(250),
                        @xTotalDisplay int OUTPUT
                    ';


                    EXEC sp_executesql
                        @countSql,
                        @countParamList,
                        @xSearchText = @SearchText,
                        @xTotalDisplay = @TotalDisplay OUTPUT;


                    -- ==========================================
                    -- 7. MAIN QUERY EXECUTION
                    -- ==========================================

                    DECLARE @paramList nvarchar(MAX);


                    SET @paramList = '
                        @xSearchText nvarchar(250),
                        @xPageIndex int,
                        @xPageSize int
                    ';


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
                DROP PROCEDURE IF EXISTS
                    [dbo].[GetOrders]
                """);
        }
    }
}




