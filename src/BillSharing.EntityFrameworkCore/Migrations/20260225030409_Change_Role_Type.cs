using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BillSharing.Migrations
{
    /// <inheritdoc />
    public partial class Change_Role_Type : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                @"UPDATE AppGroupMembers SET Role = 
                    CASE 
                        WHEN Role = 'owner' THEN '0'
                        WHEN Role = 'member' THEN '1'
                    END"
            );

            migrationBuilder.AlterColumn<int>(
                name: "Role",
                table: "AppGroupMembers",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "AppGroupMembers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
