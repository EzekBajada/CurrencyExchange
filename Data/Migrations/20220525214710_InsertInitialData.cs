using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CurrencyExchange.Data.Migrations
{
    public partial class InsertInitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                            table: "Clients",
                            columns: new[] { "ClientName", "BaseCurrency" },
                            values: new object[] { "Medirect", "EUR" });

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
