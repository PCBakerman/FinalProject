using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.Migrations
{
    public partial class addressForUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AddressLine",
                table: "UserInventories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "City",
                table: "UserInventories",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsAddressPublic",
                table: "UserInventories",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "State",
                table: "UserInventories",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Zip",
                table: "UserInventories",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AddressLine",
                table: "UserInventories");

            migrationBuilder.DropColumn(
                name: "City",
                table: "UserInventories");

            migrationBuilder.DropColumn(
                name: "IsAddressPublic",
                table: "UserInventories");

            migrationBuilder.DropColumn(
                name: "State",
                table: "UserInventories");

            migrationBuilder.DropColumn(
                name: "Zip",
                table: "UserInventories");
        }
    }
}
