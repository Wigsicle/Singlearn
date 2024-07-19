using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Singlearn.Migrations
{
    /// <inheritdoc />
    public partial class EditSubmissionEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Submissions_New",
            columns: table => new
            {
                submission_id = table.Column<int>(nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                class_id = table.Column<string>(type: "nvarchar(450)",nullable: false),
                homework_id = table.Column<int>(nullable: false),
                originalFilename = table.Column<byte[]>(nullable: true),
                annotatedFilename = table.Column<byte[]>(nullable: true),
                feedback = table.Column<string>(nullable: true),
                grade = table.Column<string>(nullable: true),
                status = table.Column<string>(nullable: true),
                visibility = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Submissions_New", x => x.submission_id);
                table.ForeignKey(
                    name: "FK_Classes_class_id2",
                    column: x => x.class_id,
                    principalTable: "Classes",
                    principalColumn: "class_id",
                    onDelete: ReferentialAction.NoAction);
                table.ForeignKey(
                    name: "FK_Homework_homework_id3",
                    column: x => x.homework_id,
                    principalTable: "Homeworks",
                    principalColumn: "homework_id",
                    onDelete: ReferentialAction.NoAction);
            });

            // Step 2: Copy data from the old table to the new table
            migrationBuilder.Sql(@"
            INSERT INTO Submissions_New (class_id, homework_id, originalFilename, annotatedFilename, feedback, grade, status, visibility)
            SELECT class_id, homework_id, originalFilename, annotatedFilename, feedback, grade, status, visibility
            FROM Submissions
        ");

            // Step 3: Drop the old table
            migrationBuilder.DropTable(name: "Submissions");

            // Step 4: Rename the new table to the old table's name
            migrationBuilder.RenameTable(name: "Submissions_New", newName: "Submissions");



        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
            name: "Submissions_Old",
            columns: table => new
            {
                submission_id = table.Column<int>(nullable: false),
                class_id = table.Column<string>(nullable: false),
                homework_id = table.Column<int>(nullable: false),
                originalFilename = table.Column<byte[]>(nullable: true),
                annotatedFilename = table.Column<byte[]>(nullable: true),
                feedback = table.Column<string>(nullable: true),
                grade = table.Column<string>(nullable: true),
                status = table.Column<string>(nullable: true),
                visibility = table.Column<bool>(nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Submissions_Old", x => x.submission_id);
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

            // Step 2: Copy data back from the new table to the old table
            migrationBuilder.Sql(@"
            INSERT INTO Submissions_Old (submission_id, class_id, homework_id, originalFilename, annotatedFilename, feedback, grade, status, visibility)
            SELECT submission_id, class_id, homework_id, originalFilename, annotatedFilename, feedback, grade, status, visibility
            FROM Submissions
        ");

            // Step 3: Drop the new table
            migrationBuilder.DropTable(name: "Submissions");

            // Step 4: Rename the old table back to the original name
            migrationBuilder.RenameTable(name: "Submissions_Old", newName: "Submissions");
        
    }
    }
}
