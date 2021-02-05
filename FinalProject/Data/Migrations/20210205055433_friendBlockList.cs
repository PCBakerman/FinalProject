using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.Migrations
{
    public partial class friendBlockList : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FriendBlockItems",
                columns: table => new
                {
                    UserInventoryOneId = table.Column<int>(nullable: false),
                    UserInventorTwoId = table.Column<int>(nullable: false),
                    UserInventoryTwoId = table.Column<int>(nullable: true),
                    Status = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FriendBlockItems", x => new { x.UserInventoryOneId, x.UserInventorTwoId });
                    table.ForeignKey(
                        name: "FK_FriendBlockItems_UserInventories_UserInventoryOneId",
                        column: x => x.UserInventoryOneId,
                        principalTable: "UserInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_FriendBlockItems_UserInventories_UserInventoryTwoId",
                        column: x => x.UserInventoryTwoId,
                        principalTable: "UserInventories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FriendBlockItems_UserInventoryTwoId",
                table: "FriendBlockItems",
                column: "UserInventoryTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FriendBlockItems");
        }
    }
}
