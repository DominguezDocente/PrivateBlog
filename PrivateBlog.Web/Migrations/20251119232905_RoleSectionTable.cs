using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateBlog.Web.Migrations
{
    /// <inheritdoc />
    public partial class RoleSectionTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RoleSections",
                columns: table => new
                {
                    PrivateBlogRoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SectionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoleSections", x => new { x.SectionId, x.PrivateBlogRoleId });
                    table.ForeignKey(
                        name: "FK_RoleSections_PrivateBlogRoles_PrivateBlogRoleId",
                        column: x => x.PrivateBlogRoleId,
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
                name: "IX_Permissions_Name",
                table: "Permissions",
                column: "Name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_RoleSections_PrivateBlogRoleId",
                table: "RoleSections",
                column: "PrivateBlogRoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoleSections");

            migrationBuilder.DropIndex(
                name: "IX_Permissions_Name",
                table: "Permissions");
        }
    }
}
