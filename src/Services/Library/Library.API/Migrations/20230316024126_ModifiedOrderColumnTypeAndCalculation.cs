using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedOrderColumnTypeAndCalculation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Order",
                table: "Units",
                type: "real",
                nullable: false,
                computedColumnSql: "([OrderNumerator] * 1.0 / [OrderDenominator])",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldComputedColumnSql: "[OrderNumerator] / [OrderDenominator] * 1.0",
                oldStored: true);

            migrationBuilder.AlterColumn<float>(
                name: "Order",
                table: "Lessons",
                type: "real",
                nullable: false,
                computedColumnSql: "([OrderNumerator] * 1.0 / [OrderDenominator])",
                stored: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,0)",
                oldComputedColumnSql: "[OrderNumerator] / [OrderDenominator] * 1.0",
                oldStored: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Order",
                table: "Units",
                type: "decimal(18,0)",
                nullable: false,
                computedColumnSql: "[OrderNumerator] / [OrderDenominator] * 1.0",
                stored: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldComputedColumnSql: "([OrderNumerator] * 1.0 / [OrderDenominator])",
                oldStored: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Order",
                table: "Lessons",
                type: "decimal(18,0)",
                nullable: false,
                computedColumnSql: "[OrderNumerator] / [OrderDenominator] * 1.0",
                stored: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldComputedColumnSql: "([OrderNumerator] * 1.0 / [OrderDenominator])",
                oldStored: true);
        }
    }
}
