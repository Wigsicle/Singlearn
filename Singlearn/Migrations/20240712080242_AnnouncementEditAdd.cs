using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class AnnouncementEditAdd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "category",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "News"); // Replace with a suitable default value


            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "Visible"); // Replace with a suitable default value

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
