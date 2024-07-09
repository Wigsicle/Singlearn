using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class NullableAnnouncements : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop the foreign key constraints
            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_subject_id",
                table: "Announcements");

            migrationBuilder.DropForeignKey(
                name: "FK_Announcements_class_id",
                table: "Announcements");

            // Alter the columns to be nullable
            migrationBuilder.AlterColumn<int>(
                name: "subject_id",
                table: "Announcements",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "class_id",
                table: "Announcements",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: false);

            // Recreate the foreign key constraints
            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_subject_id",
                table: "Announcements",
                column: "subject_id",
                principalTable: "Subjects",
                principalColumn: "subject_id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Announcements_class_id",
                table: "Announcements",
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
