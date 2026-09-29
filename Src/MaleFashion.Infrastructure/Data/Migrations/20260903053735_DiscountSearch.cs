using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DiscountSearch : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var sql = """
            CREATE OR ALTER PROCEDURE [dbo].[GetDiscounts]
                @PageIndex int,
                @PageSize int,
                @OrderBy nvarchar(50),

                @DiscountName nvarchar(250) = NULL,
                @Code nvarchar(250) = NULL,
                @StartAt datetime2 = NULL,
                @EndAt datetime2 = NULL,
                @IsActive bit = NULL,

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
                FROM dbo.Discounts;


                -- ==========================================
                -- FILTERED COUNT
                -- ==========================================

                SET @countSql = '
                    SELECT @xTotalDisplay = COUNT(*)
                    FROM dbo.Discounts
                    WHERE 1 = 1
                ';


                -- ==========================================
                -- DISCOUNT NAME SEARCH
                -- ==========================================

                IF @DiscountName IS NOT NULL
                   AND LTRIM(RTRIM(@DiscountName)) <> ''
                BEGIN

                    SET @countSql = @countSql + '
                        AND DiscountName LIKE ''%'' + @xDiscountName + ''%''
                    ';

                END;


                -- ==========================================
                -- CODE SEARCH
                -- ==========================================

                IF @Code IS NOT NULL
                   AND LTRIM(RTRIM(@Code)) <> ''
                BEGIN

                    SET @countSql = @countSql + '
                        AND Code LIKE ''%'' + @xCode + ''%''
                    ';

                END;


                -- ==========================================
                -- START AT SEARCH
                -- ==========================================

                IF @StartAt IS NOT NULL
                BEGIN

                    SET @countSql = @countSql + '
                        AND StartAt >= @xStartAt
                    ';

                END;


                -- ==========================================
                -- END AT SEARCH
                -- ==========================================

                IF @EndAt IS NOT NULL
                BEGIN

                    SET @countSql = @countSql + '
                        AND EndAt <= @xEndAt
                    ';

                END;


                -- ==========================================
                -- STATUS SEARCH
                -- ==========================================

                IF @IsActive IS NOT NULL
                BEGIN

                    SET @countSql = @countSql + '
                        AND IsActive = @xIsActive
                    ';

                END;


                -- ==========================================
                -- MAIN QUERY
                -- ==========================================

                SET @sql = '
                    SELECT *
                    FROM dbo.Discounts
                    WHERE 1 = 1
                ';


                -- ==========================================
                -- DISCOUNT NAME SEARCH
                -- ==========================================

                IF @DiscountName IS NOT NULL
                   AND LTRIM(RTRIM(@DiscountName)) <> ''
                BEGIN

                    SET @sql = @sql + '
                        AND DiscountName LIKE ''%'' + @xDiscountName + ''%''
                    ';

                END;


                -- ==========================================
                -- CODE SEARCH
                -- ==========================================

                IF @Code IS NOT NULL
                   AND LTRIM(RTRIM(@Code)) <> ''
                BEGIN

                    SET @sql = @sql + '
                        AND Code LIKE ''%'' + @xCode + ''%''
                    ';

                END;


                -- ==========================================
                -- START AT SEARCH
                -- ==========================================

                IF @StartAt IS NOT NULL
                BEGIN

                    SET @sql = @sql + '
                        AND StartAt >= @xStartAt
                    ';

                END;


                -- ==========================================
                -- END AT SEARCH
                -- ==========================================

                IF @EndAt IS NOT NULL
                BEGIN

                    SET @sql = @sql + '
                        AND EndAt <= @xEndAt
                    ';

                END;


                -- ==========================================
                -- STATUS SEARCH
                -- ==========================================

                IF @IsActive IS NOT NULL
                BEGIN

                    SET @sql = @sql + '
                        AND IsActive = @xIsActive
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
                    @xDiscountName nvarchar(250),
                    @xCode nvarchar(250),
                    @xStartAt datetime2,
                    @xEndAt datetime2,
                    @xIsActive bit,
                    @xTotalDisplay int OUTPUT
                ';


                -- ==========================================
                -- EXECUTE FILTERED COUNT
                -- ==========================================

                EXEC sp_executesql
                    @countSql,
                    @countParamList,
                    @DiscountName,
                    @Code,
                    @StartAt,
                    @EndAt,
                    @IsActive,
                    @xTotalDisplay =
                        @TotalDisplay OUTPUT;


                -- ==========================================
                -- MAIN QUERY PARAMETERS
                -- ==========================================

                SET @paramList = '
                    @xDiscountName nvarchar(250),
                    @xCode nvarchar(250),
                    @xStartAt datetime2,
                    @xEndAt datetime2,
                    @xIsActive bit,
                    @xPageIndex int,
                    @xPageSize int
                ';


                -- ==========================================
                -- EXECUTE MAIN QUERY
                -- ==========================================

                EXEC sp_executesql
                    @sql,
                    @paramList,
                    @DiscountName,
                    @Code,
                    @StartAt,
                    @EndAt,
                    @IsActive,
                    @PageIndex,
                    @PageSize;

            END
            """;

            migrationBuilder.Sql(sql);


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                 """
                DROP PROCEDURE IF EXISTS [dbo].[GetDiscounts]
                """
             );
        }
    }
}
