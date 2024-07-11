using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class AddPDFFileToMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
                name: "pdf_file",
                table: "Materials",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
