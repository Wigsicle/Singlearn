using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class AddHomeworkEntitty : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Homeworks",
               columns: table => new
               {
                   homework_id = table.Column<int>(type: "int", nullable: false)
                       .Annotation("SqlServer:Identity", "1, 1"),
                   subject_id = table.Column<int>(type: "int", nullable: false),
                   title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   attachment = table.Column<string>(type: "nvarchar(max)", nullable: true),
                   startdate = table.Column<DateTime>(type: "datetime2", nullable: false),
                   enddate = table.Column<DateTime>(type: "datetime2", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Homeworks", x => x.homework_id);
                   table.ForeignKey(
                       name: "FK_SubjectTeacherClasses_subject_id2",
                       column: x => x.subject_id,
                       principalTable: "Subjects",
                       principalColumn: "subject_id",
                       onDelete: ReferentialAction.NoAction);
               });


        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
