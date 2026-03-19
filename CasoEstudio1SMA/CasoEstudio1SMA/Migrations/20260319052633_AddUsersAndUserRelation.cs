using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoryApi.Migrations
{
    /// <inheritdoc />
    public partial class AddUsersAndUserRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Apellidos = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    AvatarId = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Nombre", "Apellidos", "Email", "AvatarId" },
                values: new object[] { "Sin", "Asignar", "sin.asignar@local", 1 });

            migrationBuilder.DropColumn(
                name: "AsignadoA",
                table: "UserStories");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "UserStories",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_UserStories_UserId",
                table: "UserStories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserStories_Users_UserId",
                table: "UserStories",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserStories_Users_UserId",
                table: "UserStories");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropIndex(
                name: "IX_UserStories_UserId",
                table: "UserStories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "UserStories");

            migrationBuilder.AddColumn<string>(
                name: "AsignadoA",
                table: "UserStories",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");
        }
    }
}
