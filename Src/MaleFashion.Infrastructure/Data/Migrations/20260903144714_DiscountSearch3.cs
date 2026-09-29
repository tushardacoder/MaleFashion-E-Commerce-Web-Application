using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaleFashion.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class DiscountSearch3 : Migration
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
                -- TOTAL RECORDS
                --------------------------------------------------

                SELECT @Total = COUNT(*)
                FROM dbo.Discount;


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
                -- START DATE
                --------------------------------------------------

                IF @StartAt IS NOT NULL
                BEGIN
                    SET @countSql += '
                        AND StartAt >= @xStartAt
                    ';
                END;


                --------------------------------------------------
                -- END DATE
                --------------------------------------------------

                IF @EndAt IS NOT NULL
                BEGIN
                    SET @countSql += '
                        AND EndAt <= @xEndAt
                    ';
                END;


                --------------------------------------------------
                -- ACTIVE
                --------------------------------------------------

                IF @IsActive IS NOT NULL
                BEGIN
                    SET @countSql += '
                        AND IsActive = @xIsActive
                    ';
                END;


                --------------------------------------------------
                -- COUNT PARAMETERS
                --------------------------------------------------

                SET @countParamList = '
                    @xSearchText nvarchar(250),
                    @xStartAt datetime2,
                    @xEndAt datetime2,
                    @xIsActive bit,
                    @xTotalDisplay int OUTPUT
                ';


                --------------------------------------------------
                -- EXECUTE FILTERED COUNT
                --------------------------------------------------

                EXEC sp_executesql
                    @countSql,
                    @countParamList,
                    @xSearchText = @SearchText,
                    @xStartAt = @StartAt,
                    @xEndAt = @EndAt,
                    @xIsActive = @IsActive,
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
                -- START DATE
                --------------------------------------------------

                IF @StartAt IS NOT NULL
                BEGIN
                    SET @sql += '
                        AND StartAt >= @xStartAt
                    ';
                END;


                --------------------------------------------------
                -- END DATE
                --------------------------------------------------

                IF @EndAt IS NOT NULL
                BEGIN
                    SET @sql += '
                        AND EndAt <= @xEndAt
                    ';
                END;


                --------------------------------------------------
                -- ACTIVE
                --------------------------------------------------

                IF @IsActive IS NOT NULL
                BEGIN
                    SET @sql += '
                        AND IsActive = @xIsActive
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
                    @xStartAt datetime2,
                    @xEndAt datetime2,
                    @xIsActive bit,
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
