using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistance.Migrations
{
    public partial class EstimatedTimeHRS : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedCompletionTime",
                table: "LearningPaths");

            migrationBuilder.AddColumn<int>(
                name: "EstimatedCompletionTimeInHrs",
                table: "LearningPaths",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedCompletionTimeInHrs",
                table: "LearningPaths");

            migrationBuilder.AddColumn<TimeSpan>(
                name: "EstimatedCompletionTime",
                table: "LearningPaths",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
