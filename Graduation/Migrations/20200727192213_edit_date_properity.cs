using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Graduation.Migrations
{
    public partial class edit_date_properity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "date",
                table: "fridgeTempretures",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "date",
                table: "fridgeTempretures",
                nullable: true,
                oldClrType: typeof(DateTime));
        }
    }
}
