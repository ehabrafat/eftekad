using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eftekad.Migrations
{
    /// <inheritdoc />
    public partial class link_member_user : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MemberId",
                table: "Users",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_MemberId",
                table: "Users",
                column: "MemberId",
                unique: true,
                filter: "[MemberId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Members_MemberId",
                table: "Users",
                column: "MemberId",
                principalTable: "Members",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_Members_MemberId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_MemberId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "MemberId",
                table: "Users");
        }
    }
}
