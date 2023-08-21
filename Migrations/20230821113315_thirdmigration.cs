using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WeddingPlanner2.Migrations
{
    public partial class thirdmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingGuest_Users_UserId",
                table: "WeddingGuest");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingGuest_Weddings_WeddingId",
                table: "WeddingGuest");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeddingGuest",
                table: "WeddingGuest");

            migrationBuilder.RenameTable(
                name: "WeddingGuest",
                newName: "WeddingGuests");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingGuest_WeddingId",
                table: "WeddingGuests",
                newName: "IX_WeddingGuests_WeddingId");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingGuest_UserId",
                table: "WeddingGuests",
                newName: "IX_WeddingGuests_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeddingGuests",
                table: "WeddingGuests",
                column: "WeddingGuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingGuests_Users_UserId",
                table: "WeddingGuests",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingGuests_Weddings_WeddingId",
                table: "WeddingGuests",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WeddingGuests_Users_UserId",
                table: "WeddingGuests");

            migrationBuilder.DropForeignKey(
                name: "FK_WeddingGuests_Weddings_WeddingId",
                table: "WeddingGuests");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WeddingGuests",
                table: "WeddingGuests");

            migrationBuilder.RenameTable(
                name: "WeddingGuests",
                newName: "WeddingGuest");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingGuests_WeddingId",
                table: "WeddingGuest",
                newName: "IX_WeddingGuest_WeddingId");

            migrationBuilder.RenameIndex(
                name: "IX_WeddingGuests_UserId",
                table: "WeddingGuest",
                newName: "IX_WeddingGuest_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_WeddingGuest",
                table: "WeddingGuest",
                column: "WeddingGuestId");

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingGuest_Users_UserId",
                table: "WeddingGuest",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WeddingGuest_Weddings_WeddingId",
                table: "WeddingGuest",
                column: "WeddingId",
                principalTable: "Weddings",
                principalColumn: "WeddingId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
