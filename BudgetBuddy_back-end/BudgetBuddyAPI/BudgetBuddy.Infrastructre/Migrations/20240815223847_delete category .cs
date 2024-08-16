using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BudgetBuddy.Infrastructure.Migrations
{
    public partial class deletecategory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Expense_Category_CategoryID",
                table: "Expense");

            migrationBuilder.DropTable(
                name: "Category");

            migrationBuilder.DropIndex(
                name: "IX_Expense_CategoryID",
                table: "Expense");

            migrationBuilder.DropColumn(
                name: "CategoryID",
                table: "Expense");

            migrationBuilder.AddColumn<string>(
                name: "Tag",
                table: "Expense",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tag",
                table: "Expense");

            migrationBuilder.AddColumn<string>(
                name: "CategoryID",
                table: "Expense",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Category",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Category", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Expense_CategoryID",
                table: "Expense",
                column: "CategoryID");

            migrationBuilder.AddForeignKey(
                name: "FK_Expense_Category_CategoryID",
                table: "Expense",
                column: "CategoryID",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
