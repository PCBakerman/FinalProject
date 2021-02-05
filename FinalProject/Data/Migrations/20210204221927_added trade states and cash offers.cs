using Microsoft.EntityFrameworkCore.Migrations;

namespace FinalProject.Data.Migrations
{
    public partial class addedtradestatesandcashoffers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TradeListings",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ApplicationUserId = table.Column<string>(nullable: true),
                    CardId = table.Column<int>(nullable: false),
                    CashOffer = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeListings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeListings_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TradeListings_Cards_CardId",
                        column: x => x.CardId,
                        principalTable: "Cards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TradeOffers",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TradeListingOneId = table.Column<int>(nullable: false),
                    TradeListingTwoId = table.Column<int>(nullable: false),
                    TradeState = table.Column<int>(nullable: false),
                    TradeListingOneRating = table.Column<int>(nullable: false),
                    TradeListingTwoRating = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TradeOffers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TradeOffers_TradeListings_TradeListingOneId",
                        column: x => x.TradeListingOneId,
                        principalTable: "TradeListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_TradeOffers_TradeListings_TradeListingTwoId",
                        column: x => x.TradeListingTwoId,
                        principalTable: "TradeListings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TradeListings_ApplicationUserId",
                table: "TradeListings",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeListings_CardId",
                table: "TradeListings",
                column: "CardId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeOffers_TradeListingOneId",
                table: "TradeOffers",
                column: "TradeListingOneId");

            migrationBuilder.CreateIndex(
                name: "IX_TradeOffers_TradeListingTwoId",
                table: "TradeOffers",
                column: "TradeListingTwoId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TradeOffers");

            migrationBuilder.DropTable(
                name: "TradeListings");
        }
    }
}
