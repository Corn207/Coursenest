using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Library.API.Migrations
{
	/// <inheritdoc />
	public partial class ChangeToRequiredMinutes : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AddColumn<int>(
				name: "RequiredMinutes",
				table: "Units",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.Sql(
			@"
                UPDATE Units
                SET RequiredMinutes = DATEPART(minute, [RequiredTime]);
            ");

			migrationBuilder.DropColumn(
				name: "RequiredTime",
				table: "Units");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropColumn(
				name: "RequiredMinutes",
				table: "Units");

			migrationBuilder.AddColumn<TimeSpan>(
				name: "RequiredTime",
				table: "Units",
				type: "time",
				nullable: false,
				defaultValue: new TimeSpan(0, 0, 0, 0, 0));
		}
	}
}
