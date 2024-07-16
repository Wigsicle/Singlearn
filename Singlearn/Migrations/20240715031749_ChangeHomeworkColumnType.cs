using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class ChangeHomeworkColumnType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "attachment",
                table: "Homeworks");


            migrationBuilder.AddColumn<byte[]>(
                name: "attachment",
                table: "Homeworks",
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
