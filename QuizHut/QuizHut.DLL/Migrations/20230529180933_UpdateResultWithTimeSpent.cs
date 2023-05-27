using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuizHut.DLL.Migrations
{
    /// <inheritdoc />
    public partial class UpdateResultWithTimeSpent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSpent",
                table: "Results",
                type: "time(6)",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeSpent",
                table: "Results");
        }
    }
}
