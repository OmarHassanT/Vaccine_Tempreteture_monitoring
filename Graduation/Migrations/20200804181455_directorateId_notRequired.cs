using Microsoft.EntityFrameworkCore.Migrations;

namespace Graduation.Migrations
{
    public partial class directorateId_notRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Directorates_DirectorateId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorateId",
                table: "AspNetUsers",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Directorates_DirectorateId",
                table: "AspNetUsers",
                column: "DirectorateId",
                principalTable: "Directorates",
                principalColumn: "DirectorateId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Directorates_DirectorateId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<int>(
                name: "DirectorateId",
                table: "AspNetUsers",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Directorates_DirectorateId",
                table: "AspNetUsers",
                column: "DirectorateId",
                principalTable: "Directorates",
                principalColumn: "DirectorateId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
