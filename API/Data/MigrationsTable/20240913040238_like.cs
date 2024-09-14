using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API.Data.MigrationsTable
{
    /// <inheritdoc />
    public partial class like : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Likes",
                columns: table => new
                {
                    SourceUserId = table.Column<int>(type: "INTEGER", nullable: false),
                    TagetUserId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Likes", x => new { x.SourceUserId, x.TagetUserId });
                    table.ForeignKey(
                        name: "FK_Likes_Users_SourceUserId",
                        column: x => x.SourceUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Likes_Users_TagetUserId",
                        column: x => x.TagetUserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Likes_TagetUserId",
                table: "Likes",
                column: "TagetUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Likes");
        }
    }
}
