using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class AddAnnouncementUrl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "url",
                table: "Announcements",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           
        }
    }
}
