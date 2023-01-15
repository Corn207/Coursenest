using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserData.API.Migrations
{
	/// <inheritdoc />
	public partial class InitialCreate : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "EnrolledCourses",
				columns: table => new
				{
					EnrolledCourseId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CompletedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
					CourseId = table.Column<int>(type: "int", nullable: false),
					StudierUserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_EnrolledCourses", x => x.EnrolledCourseId);
				});

			migrationBuilder.CreateTable(
				name: "FollowedCourses",
				columns: table => new
				{
					FollowedCourseId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CourseId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FollowedCourses", x => x.FollowedCourseId);
				});

			migrationBuilder.CreateTable(
				name: "FollowedTopics",
				columns: table => new
				{
					FollowedTopicId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					TopicId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_FollowedTopics", x => x.FollowedTopicId);
				});

			migrationBuilder.CreateTable(
				name: "Ratings",
				columns: table => new
				{
					RatingId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Stars = table.Column<int>(type: "int", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					CourseId = table.Column<int>(type: "int", nullable: false),
					UserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Ratings", x => x.RatingId);
				});

			migrationBuilder.CreateTable(
				name: "Submissions",
				columns: table => new
				{
					SubmissionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
					LessonName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					CourseName = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Created = table.Column<DateTime>(type: "datetime2", nullable: false),
					Graded = table.Column<DateTime>(type: "datetime2", nullable: true),
					Elapsed = table.Column<TimeSpan>(type: "time", nullable: false),
					TimeLimit = table.Column<TimeSpan>(type: "time", nullable: false),
					OwnerUserId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Submissions", x => x.SubmissionId);
				});

			migrationBuilder.CreateTable(
				name: "CompletedUnits",
				columns: table => new
				{
					CompletedUnitId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					UnitId = table.Column<int>(type: "int", nullable: false),
					EnrolledCourseId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_CompletedUnits", x => x.CompletedUnitId);
					table.ForeignKey(
						name: "FK_CompletedUnits_EnrolledCourses_EnrolledCourseId",
						column: x => x.EnrolledCourseId,
						principalTable: "EnrolledCourses",
						principalColumn: "EnrolledCourseId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Comments",
				columns: table => new
				{
					CommentId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					OwnerUserId = table.Column<int>(type: "int", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Comments", x => x.CommentId);
					table.ForeignKey(
						name: "FK_Comments_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Criteria",
				columns: table => new
				{
					CriterionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Criteria", x => x.CriterionId);
					table.ForeignKey(
						name: "FK_Criteria_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Questions",
				columns: table => new
				{
					QuestionId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Point = table.Column<int>(type: "int", nullable: false),
					SubmissionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Questions", x => x.QuestionId);
					table.ForeignKey(
						name: "FK_Questions_Submissions_SubmissionId",
						column: x => x.SubmissionId,
						principalTable: "Submissions",
						principalColumn: "SubmissionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Checkpoints",
				columns: table => new
				{
					CheckpointId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Point = table.Column<int>(type: "int", nullable: false),
					IsChecked = table.Column<bool>(type: "bit", nullable: false),
					CriterionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Checkpoints", x => x.CheckpointId);
					table.ForeignKey(
						name: "FK_Checkpoints_Criteria_CriterionId",
						column: x => x.CriterionId,
						principalTable: "Criteria",
						principalColumn: "CriterionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateTable(
				name: "Answers",
				columns: table => new
				{
					AnswerId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					Content = table.Column<string>(type: "nvarchar(max)", nullable: false),
					IsCorrect = table.Column<bool>(type: "bit", nullable: true),
					IsChosen = table.Column<bool>(type: "bit", nullable: true),
					QuestionId = table.Column<int>(type: "int", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Answers", x => x.AnswerId);
					table.ForeignKey(
						name: "FK_Answers_Questions_QuestionId",
						column: x => x.QuestionId,
						principalTable: "Questions",
						principalColumn: "QuestionId",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_Answers_QuestionId",
				table: "Answers",
				column: "QuestionId");

			migrationBuilder.CreateIndex(
				name: "IX_Checkpoints_CriterionId",
				table: "Checkpoints",
				column: "CriterionId");

			migrationBuilder.CreateIndex(
				name: "IX_Comments_SubmissionId",
				table: "Comments",
				column: "SubmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_CompletedUnits_EnrolledCourseId",
				table: "CompletedUnits",
				column: "EnrolledCourseId");

			migrationBuilder.CreateIndex(
				name: "IX_Criteria_SubmissionId",
				table: "Criteria",
				column: "SubmissionId");

			migrationBuilder.CreateIndex(
				name: "IX_Questions_SubmissionId",
				table: "Questions",
				column: "SubmissionId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Answers");

			migrationBuilder.DropTable(
				name: "Checkpoints");

			migrationBuilder.DropTable(
				name: "Comments");

			migrationBuilder.DropTable(
				name: "CompletedUnits");

			migrationBuilder.DropTable(
				name: "FollowedCourses");

			migrationBuilder.DropTable(
				name: "FollowedTopics");

			migrationBuilder.DropTable(
				name: "Ratings");

			migrationBuilder.DropTable(
				name: "Questions");

			migrationBuilder.DropTable(
				name: "Criteria");

			migrationBuilder.DropTable(
				name: "EnrolledCourses");

			migrationBuilder.DropTable(
				name: "Submissions");
		}
	}
}
