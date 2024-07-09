using Microsoft.EntityFrameworkCore.Migrations;
using System.Reflection.Metadata;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class NullableMaterials : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_Materials_class_id",
                table: "Materials");

            // Alter the columns to be nullable
            migrationBuilder.AlterColumn<string>(
                name: "class_id",
                table: "Materials",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "link",
                table: "Materials",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: false);

            migrationBuilder.AddColumn<Blob>(
                name: "data",
                table: "Materials",
                type: "varbinary(max)",
                nullable: true,
                defaultValue: "");

            // Recreate the foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Materials_class_id",
                table: "Materials",
                column: "class_id",
                principalTable: "Classes",
                principalColumn: "class_id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
