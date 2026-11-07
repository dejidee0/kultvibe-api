using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KultVibe.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWaitlistUserTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "PrimaryVertical",
                table: "WaitlistEntries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<string>(
                name: "CompanyName",
                table: "WaitlistEntries",
                type: "nvarchar(150)",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Interests",
                table: "WaitlistEntries",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PrimaryGame",
                table: "WaitlistEntries",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserType",
                table: "WaitlistEntries",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Gamer");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CompanyName",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "Interests",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "PrimaryGame",
                table: "WaitlistEntries");

            migrationBuilder.DropColumn(
                name: "UserType",
                table: "WaitlistEntries");

            migrationBuilder.AlterColumn<string>(
                name: "PrimaryVertical",
                table: "WaitlistEntries",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50,
                oldNullable: true);
        }
    }
}
