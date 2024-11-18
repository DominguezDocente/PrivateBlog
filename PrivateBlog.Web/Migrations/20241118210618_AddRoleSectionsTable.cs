using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateBlog.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddRoleSectionsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleSections",
                columns: table => new
                {
                    RoleId = table.Column<int>(type: "int", nullable: false),
                    SectionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSections", x => new { x.RoleId, x.SectionId });
                    table.ForeignKey(
                        name: "FK_RoleSections_PrivateBlogRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "PrivateBlogRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoleSections_Sections_SectionId",
                        column: x => x.SectionId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoleSections_SectionId",
                table: "RoleSections",
                column: "SectionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleSections");
        }
    }
}
