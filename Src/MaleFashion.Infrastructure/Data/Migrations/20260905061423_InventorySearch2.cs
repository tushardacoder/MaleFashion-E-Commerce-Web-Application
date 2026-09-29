using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InventorySearch2 : Migration
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

                            -- PRODUCT NAME
                            WHEN 'ProductName ASC'
                                THEN 'P.ProductName ASC'

                            WHEN 'ProductName DESC'
                                THEN 'P.ProductName DESC'


                            -- COLOR
                            WHEN 'Color ASC'
                                THEN 'PV.Color ASC'

                            WHEN 'Color DESC'
                                THEN 'PV.Color DESC'


                            -- SIZE
                            WHEN 'Size ASC'
                                THEN 'PV.Size ASC'

                            WHEN 'Size DESC'
                                THEN 'PV.Size DESC'


                            -- QUANTITY
                            WHEN 'Quantity ASC'
                                THEN 'I.Quantity ASC'

                            WHEN 'Quantity DESC'
                                THEN 'I.Quantity DESC'


                            -- ACTIVE STATUS
                            WHEN 'IsActive ASC'
                                THEN 'I.IsActive ASC'

                            WHEN 'IsActive DESC'
                                THEN 'I.IsActive DESC'


                            -- UPDATED DATE
                            WHEN 'UpdatedAt ASC'
                                THEN 'I.UpdatedAt ASC'

                            WHEN 'UpdatedAt DESC'
                                THEN 'I.UpdatedAt DESC'


                            -- ID
                            WHEN 'Id ASC'
                                THEN 'I.Id ASC'

                            WHEN 'Id DESC'
                                THEN 'I.Id DESC'


                            -- DEFAULT
                            ELSE 'I.Id ASC'

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

