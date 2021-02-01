using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.Migrations
{
    public partial class AddedImageToDeck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "ImageBytes",
                table: "Decks",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageBytes",
                table: "Decks");
        }
    }
}
