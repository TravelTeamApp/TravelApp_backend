using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplication2.Migrations
{
    /// <inheritdoc />
    public partial class AddUserPlaceTypeRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlaceTypeId",
                table: "Places",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "PlaceTypes",
                columns: table => new
                {
                    PlaceTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PlaceTypeName = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlaceTypes", x => x.PlaceTypeId);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "UserPlaceTypes",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false),
                    PlaceTypeId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPlaceTypes", x => new { x.UserId, x.PlaceTypeId });
                    table.ForeignKey(
                        name: "FK_UserPlaceTypes_PlaceTypes_PlaceTypeId",
                        column: x => x.PlaceTypeId,
                        principalTable: "PlaceTypes",
                        principalColumn: "PlaceTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPlaceTypes_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "UserID",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Places_PlaceTypeId",
                table: "Places",
                column: "PlaceTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPlaceTypes_PlaceTypeId",
                table: "UserPlaceTypes",
                column: "PlaceTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Places_PlaceTypes_PlaceTypeId",
                table: "Places",
                column: "PlaceTypeId",
                principalTable: "PlaceTypes",
                principalColumn: "PlaceTypeId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Places_PlaceTypes_PlaceTypeId",
                table: "Places");

            migrationBuilder.DropTable(
                name: "UserPlaceTypes");

            migrationBuilder.DropTable(
                name: "PlaceTypes");

            migrationBuilder.DropIndex(
                name: "IX_Places_PlaceTypeId",
                table: "Places");

            migrationBuilder.DropColumn(
                name: "PlaceTypeId",
                table: "Places");
        }
    }
}
