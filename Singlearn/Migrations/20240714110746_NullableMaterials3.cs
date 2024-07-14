using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class NullableMaterials3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
              name: "file_type",
              table: "Materials",
              type: "nvarchar(max)",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "nvarchar(max)",
              oldNullable: false);

            migrationBuilder.AlterColumn<string>(
              name: "pdf_file",
              table: "Materials",
              type: "varbinary(max)",
              nullable: true,
              oldClrType: typeof(string),
              oldType: "varbinary(max)",
              oldNullable: false);

        }
    }
}
