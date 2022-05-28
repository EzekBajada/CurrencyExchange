using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyExchange.Data.Migrations
{
    public partial class AddANewClient : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyExchangeHistories_Clients_ClientId",
                table: "CurrencyExchangeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "CurrencyExchangeHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<double>(
                name: "AmountOut",
                table: "CurrencyExchangeHistories",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<double>(
                name: "AmountIn",
                table: "CurrencyExchangeHistories",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyExchangeHistories_Clients_ClientId",
                table: "CurrencyExchangeHistories",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CurrencyExchangeHistories_Clients_ClientId",
                table: "CurrencyExchangeHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ClientId",
                table: "CurrencyExchangeHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AmountOut",
                table: "CurrencyExchangeHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<double>(
                name: "AmountIn",
                table: "CurrencyExchangeHistories",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_CurrencyExchangeHistories_Clients_ClientId",
                table: "CurrencyExchangeHistories",
                column: "ClientId",
                principalTable: "Clients",
                principalColumn: "ClientId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
