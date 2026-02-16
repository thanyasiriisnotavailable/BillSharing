using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillSharing.Migrations
{
    /// <inheritdoc />
    public partial class Added_Group_InviteCode : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "InviteCode",
                table: "AppGroups",
                type: "nvarchar(16)",
                maxLength: 16,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "InviteCodeExpiry",
                table: "AppGroups",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppGroups_InviteCode",
                table: "AppGroups",
                column: "InviteCode",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppGroups_InviteCode",
                table: "AppGroups");

            migrationBuilder.DropColumn(
                name: "InviteCode",
                table: "AppGroups");

            migrationBuilder.DropColumn(
                name: "InviteCodeExpiry",
                table: "AppGroups");
        }
    }
}
