using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MovieRating.Migrations
{
    public partial class ActorRating : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_Actor_ActorsId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorRating_Actor_ActorId",
                table: "ActorRating");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorRating_AspNetUsers_UserId",
                table: "ActorRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorRating",
                table: "ActorRating");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actor",
                table: "Actor");

            migrationBuilder.RenameTable(
                name: "ActorRating",
                newName: "ActorRatings");

            migrationBuilder.RenameTable(
                name: "Actor",
                newName: "Actors");

            migrationBuilder.RenameIndex(
                name: "IX_ActorRating_UserId",
                table: "ActorRatings",
                newName: "IX_ActorRatings_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActorRating_ActorId",
                table: "ActorRatings",
                newName: "IX_ActorRatings_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorRatings",
                table: "ActorRatings",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actors",
                table: "Actors",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_Actors_ActorsId",
                table: "ActorMovie",
                column: "ActorsId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorRatings_Actors_ActorId",
                table: "ActorRatings",
                column: "ActorId",
                principalTable: "Actors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorRatings_AspNetUsers_UserId",
                table: "ActorRatings",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ActorMovie_Actors_ActorsId",
                table: "ActorMovie");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorRatings_Actors_ActorId",
                table: "ActorRatings");

            migrationBuilder.DropForeignKey(
                name: "FK_ActorRatings_AspNetUsers_UserId",
                table: "ActorRatings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Actors",
                table: "Actors");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ActorRatings",
                table: "ActorRatings");

            migrationBuilder.RenameTable(
                name: "Actors",
                newName: "Actor");

            migrationBuilder.RenameTable(
                name: "ActorRatings",
                newName: "ActorRating");

            migrationBuilder.RenameIndex(
                name: "IX_ActorRatings_UserId",
                table: "ActorRating",
                newName: "IX_ActorRating_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ActorRatings_ActorId",
                table: "ActorRating",
                newName: "IX_ActorRating_ActorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Actor",
                table: "Actor",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ActorRating",
                table: "ActorRating",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ActorMovie_Actor_ActorsId",
                table: "ActorMovie",
                column: "ActorsId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorRating_Actor_ActorId",
                table: "ActorRating",
                column: "ActorId",
                principalTable: "Actor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ActorRating_AspNetUsers_UserId",
                table: "ActorRating",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
