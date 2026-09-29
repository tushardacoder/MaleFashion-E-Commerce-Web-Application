using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InventorySearchandaggrefrateroot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
                CREATE OR ALTER PROCEDURE [dbo].[GetInventories]
                    @PageIndex int,
                    @PageSize int,
                    @OrderBy nvarchar(100),
                    @SearchText nvarchar(250) = NULL,
                    @Total int OUTPUT,
                    @TotalDisplay int OUTPUT
                AS
                BEGIN

                    SET NOCOUNT ON;


                    -- ==========================================
                    -- VARIABLES
                    -- ==========================================

                    DECLARE @sql nvarchar(MAX);
                    DECLARE @countSql nvarchar(MAX);

                    DECLARE @paramList nvarchar(MAX);
                    DECLARE @countParamList nvarchar(MAX);

                    DECLARE @SafeOrderBy nvarchar(100);


                    -- ==========================================
                    -- SAFE ORDER BY
                    -- ==========================================

                    SET @SafeOrderBy =
                        CASE @OrderBy

                            WHEN 'Id'
                                THEN 'I.Id'

                            WHEN 'Quantity'
                                THEN 'I.Quantity'

                            WHEN 'LowStockQuantity'
                                THEN 'I.LowStockQuantity'

                            WHEN 'ProductName'
                                THEN 'P.ProductName'

                            WHEN 'Color'
                                THEN 'PV.Color'

                            WHEN 'Size'
                                THEN 'PV.Size'

                            ELSE 'I.Id'

                        END;


                    -- ==========================================
                    -- TOTAL RECORDS
                    -- ==========================================

                    SELECT
                        @Total = COUNT(*)

                    FROM Inventories;


                    -- ==========================================
                    -- FILTERED COUNT
                    -- ==========================================

                    SET @countSql = '

                        SELECT
                            @xTotalDisplay = COUNT(*)

                        FROM Inventories I

                        INNER JOIN ProductVariants PV
                            ON I.ProductVariantId = PV.Id

                        INNER JOIN Products P
                            ON PV.ProductId = P.Id

                        WHERE 1 = 1

                    ';


                    -- ==========================================
                    -- SEARCH
                    -- ==========================================

                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @countSql = @countSql + '

                            AND
                            (
                                P.ProductName LIKE
                                    ''%'' + @xSearchText + ''%''

                                OR

                                PV.Color LIKE
                                    ''%'' + @xSearchText + ''%''

                                OR

                                PV.Size LIKE
                                    ''%'' + @xSearchText + ''%''
                            )

                        ';

                    END;


                    -- ==========================================
                    -- MAIN QUERY
                    -- ==========================================

                    SET @sql = '

                        SELECT
                            I.*

                        FROM Inventories I

                        INNER JOIN ProductVariants PV
                            ON I.ProductVariantId = PV.Id

                        INNER JOIN Products P
                            ON PV.ProductId = P.Id

                        WHERE 1 = 1

                    ';


                    -- ==========================================
                    -- SEARCH
                    -- ==========================================

                    IF @SearchText IS NOT NULL
                       AND LTRIM(RTRIM(@SearchText)) <> ''
                    BEGIN

                        SET @sql = @sql + '

                            AND
                            (
                                P.ProductName LIKE
                                    ''%'' + @xSearchText + ''%''

                                OR

                                PV.Color LIKE
                                    ''%'' + @xSearchText + ''%''

                                OR

                                PV.Size LIKE
                                    ''%'' + @xSearchText + ''%''
                            )

                        ';

                    END;


                    -- ==========================================
                    -- SORTING + PAGING
                    -- ==========================================

                    SET @sql = @sql + '

                        ORDER BY ' + @SafeOrderBy + '

                        OFFSET
                            @xPageSize *
                            (@xPageIndex - 1)
                        ROWS

                        FETCH NEXT
                            @xPageSize
                        ROWS ONLY

                    ';


                    -- ==========================================
                    -- COUNT PARAMETERS
                    -- ==========================================

                    SET @countParamList = '

                        @xSearchText nvarchar(250),

                        @xTotalDisplay int OUTPUT

                    ';


                    -- ==========================================
                    -- EXECUTE COUNT QUERY
                    -- ==========================================

                    EXEC sp_executesql
                        @countSql,
                        @countParamList,
                        @SearchText,
                        @xTotalDisplay =
                            @TotalDisplay OUTPUT;


                    -- ==========================================
                    -- MAIN QUERY PARAMETERS
                    -- ==========================================

                    SET @paramList = '

                        @xSearchText nvarchar(250),

                        @xPageIndex int,

                        @xPageSize int

                    ';


                    -- ==========================================
                    -- EXECUTE MAIN QUERY
                    -- ==========================================

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
                DROP PROCEDURE IF EXISTS
                    [dbo].[GetInventories]
                """);

        }
    }
}
