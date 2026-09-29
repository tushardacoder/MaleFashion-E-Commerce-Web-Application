using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DiscountSearch2 : Migration
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

            --------------------------------------------------
            -- Total records
            --------------------------------------------------

            SELECT @Total = COUNT(*)
            FROM dbo.Discount;


            --------------------------------------------------
            -- Filtered count query
            --------------------------------------------------

            SET @countSql = '
                SELECT @xTotalDisplay = COUNT(*)
                FROM dbo.Discount
                WHERE 1 = 1
            ';


            IF @DiscountName IS NOT NULL
               AND LTRIM(RTRIM(@DiscountName)) <> ''
            BEGIN
                SET @countSql += '
                    AND DiscountName LIKE ''%'' + @xDiscountName + ''%''
                ';
            END;


            IF @Code IS NOT NULL
               AND LTRIM(RTRIM(@Code)) <> ''
            BEGIN
                SET @countSql += '
                    AND Code LIKE ''%'' + @xCode + ''%''
                ';
            END;


            IF @StartAt IS NOT NULL
            BEGIN
                SET @countSql += '
                    AND StartAt >= @xStartAt
                ';
            END;


            IF @EndAt IS NOT NULL
            BEGIN
                SET @countSql += '
                    AND EndAt <= @xEndAt
                ';
            END;


            IF @IsActive IS NOT NULL
            BEGIN
                SET @countSql += '
                    AND IsActive = @xIsActive
                ';
            END;


            --------------------------------------------------
            -- Count parameters
            --------------------------------------------------

            SET @countParamList = '
                @xDiscountName nvarchar(250),
                @xCode nvarchar(250),
                @xStartAt datetime2,
                @xEndAt datetime2,
                @xIsActive bit,
                @xTotalDisplay int OUTPUT
            ';


            --------------------------------------------------
            -- Execute filtered count
            --------------------------------------------------

            EXEC sp_executesql
                @countSql,
                @countParamList,

                @xDiscountName = @DiscountName,
                @xCode = @Code,
                @xStartAt = @StartAt,
                @xEndAt = @EndAt,
                @xIsActive = @IsActive,

                @xTotalDisplay = @TotalDisplay OUTPUT;


            --------------------------------------------------
            -- Main query
            --------------------------------------------------

            SET @sql = '
                SELECT *
                FROM dbo.Discount
                WHERE 1 = 1
            ';


            IF @DiscountName IS NOT NULL
               AND LTRIM(RTRIM(@DiscountName)) <> ''
            BEGIN
                SET @sql += '
                    AND DiscountName LIKE ''%'' + @xDiscountName + ''%''
                ';
            END;


            IF @Code IS NOT NULL
               AND LTRIM(RTRIM(@Code)) <> ''
            BEGIN
                SET @sql += '
                    AND Code LIKE ''%'' + @xCode + ''%''
                ';
            END;


            IF @StartAt IS NOT NULL
            BEGIN
                SET @sql += '
                    AND StartAt >= @xStartAt
                ';
            END;


            IF @EndAt IS NOT NULL
            BEGIN
                SET @sql += '
                    AND EndAt <= @xEndAt
                ';
            END;


            IF @IsActive IS NOT NULL
            BEGIN
                SET @sql += '
                    AND IsActive = @xIsActive
                ';
            END;


            --------------------------------------------------
            -- Ordering and paging
            --------------------------------------------------

            SET @sql += '
                ORDER BY ' + @OrderBy + '

                OFFSET @xPageSize * (@xPageIndex - 1) ROWS

                FETCH NEXT @xPageSize ROWS ONLY
            ';


            --------------------------------------------------
            -- Main query parameters
            --------------------------------------------------

            SET @paramList = '
                @xDiscountName nvarchar(250),
                @xCode nvarchar(250),
                @xStartAt datetime2,
                @xEndAt datetime2,
                @xIsActive bit,
                @xPageIndex int,
                @xPageSize int
            ';


            --------------------------------------------------
            -- Execute main query
            --------------------------------------------------

            EXEC sp_executesql
                @sql,
                @paramList,

                @xDiscountName = @DiscountName,
                @xCode = @Code,
                @xStartAt = @StartAt,
                @xEndAt = @EndAt,
                @xIsActive = @IsActive,

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
