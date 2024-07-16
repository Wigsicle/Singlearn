using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class AddSubmissionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
               name: "Submissions",
               columns: table => new
               {
                   submission_id = table.Column<int>(type:"int",nullable:false),
                   class_id = table.Column<string>(type:"nvarchar(450)",nullable:false),
                   homework_id = table.Column<int>(type: "int", nullable: false),
                   originalFilename = table.Column<byte>(type: "varbinary(max)", nullable: false),
                   annotatedFilename = table.Column<byte>(type: "varbinary(max)", nullable: false),
                   feedback = table.Column<string>(type: "nvarchar(max)", nullable: false),
                   grade = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   status = table.Column<string>(type: "nvarchar(450)", nullable: false),
                   visibility = table.Column<Boolean>(type: "bit", nullable: false)
               },
               constraints: table =>
               {
                   table.PrimaryKey("PK_Submissions", x => x.submission_id);
                   table.ForeignKey(
                       name: "FK_Classes_class_id",
                       column: x => x.class_id,
                       principalTable: "Classes",
                       principalColumn: "class_id",
                       onDelete: ReferentialAction.NoAction);
                   table.ForeignKey(
                       name: "FK_Homework_homework_id2",
                       column: x => x.homework_id,
                       principalTable: "Homeworks",
                       principalColumn: "homework_id",
                       onDelete: ReferentialAction.NoAction);
               });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
        }
    }
}
