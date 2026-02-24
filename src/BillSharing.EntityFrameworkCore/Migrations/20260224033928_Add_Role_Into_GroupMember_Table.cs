using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillSharing.Migrations
{
    /// <inheritdoc />
    public partial class Add_Role_Into_GroupMember_Table : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppItemSplits_ExpenseItemId_UserId",
                table: "AppItemSplits");

            migrationBuilder.AddColumn<string>(
                name: "Role",
                table: "AppGroupMembers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_AppItemSplits_ExpenseItemId_UserId",
                table: "AppItemSplits",
                columns: new[] { "ExpenseItemId", "UserId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppItemSplits_ExpenseItemId_UserId",
                table: "AppItemSplits");

            migrationBuilder.DropColumn(
                name: "Role",
                table: "AppGroupMembers");

            migrationBuilder.CreateIndex(
                name: "IX_AppItemSplits_ExpenseItemId_UserId",
                table: "AppItemSplits",
                columns: new[] { "ExpenseItemId", "UserId" });
        }
    }
}
