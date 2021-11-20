using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistance.Migrations
{
    public partial class LearningPathEnrollment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LearningPathEnrollment",
                columns: table => new
                {
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    LearningPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompletedLearningPath = table.Column<bool>(type: "bit", nullable: false),
                    PillarLevel = table.Column<int>(type: "int", nullable: false),
                    LearningRessourceLevel = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathEnrollment", x => new { x.ApplicationUserId, x.LearningPathId });
                    table.ForeignKey(
                        name: "FK_LearningPathEnrollment_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_LearningPathEnrollment_LearningPaths_LearningPathId",
                        column: x => x.LearningPathId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "LearningPathMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationUserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LearningPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Message = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningPathMessage_AspNetUsers_ApplicationUserId",
                        column: x => x.ApplicationUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_LearningPathMessage_LearningPaths_LearningPathId",
                        column: x => x.LearningPathId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathEnrollment_LearningPathId",
                table: "LearningPathEnrollment",
                column: "LearningPathId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathMessage_ApplicationUserId",
                table: "LearningPathMessage",
                column: "ApplicationUserId");

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathMessage_LearningPathId",
                table: "LearningPathMessage",
                column: "LearningPathId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LearningPathEnrollment");

            migrationBuilder.DropTable(
                name: "LearningPathMessage");
        }
    }
}
