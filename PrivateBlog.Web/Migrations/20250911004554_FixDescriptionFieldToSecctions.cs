using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PrivateBlog.Web.Migrations
{
    /// <inheritdoc />
    public partial class FixDescriptionFieldToSecctions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Dscription",
                table: "Sections",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Sections",
                newName: "Dscription");
        }
    }
}
