using Microsoft.EntityFrameworkCore.Migrations;

namespace Graduation.Migrations
{
    public partial class editFridgeAttribute : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "History",
                table: "fridgeTempretures");

            migrationBuilder.RenameColumn(
                name: "Temperature",
                table: "fridgeTempretures",
                newName: "temperature");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "fridgeTempretures",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Time",
                table: "fridgeTempretures",
                newName: "date");

            migrationBuilder.AlterColumn<float>(
                name: "temperature",
                table: "fridgeTempretures",
                nullable: false,
                oldClrType: typeof(double));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "temperature",
                table: "fridgeTempretures",
                newName: "Temperature");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "fridgeTempretures",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "fridgeTempretures",
                newName: "Time");

            migrationBuilder.AlterColumn<double>(
                name: "Temperature",
                table: "fridgeTempretures",
                nullable: false,
                oldClrType: typeof(float));

            migrationBuilder.AddColumn<string>(
                name: "History",
                table: "fridgeTempretures",
                nullable: true);
        }
    }
}
