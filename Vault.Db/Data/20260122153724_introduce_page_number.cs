using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Vault.Db.Data
{
    /// <inheritdoc />
    public partial class introduce_page_number : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "page_number",
                table: "Document",
                type: "integer",
                maxLength: 10000,
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "page_number",
                table: "Document");
        }
    }
}
