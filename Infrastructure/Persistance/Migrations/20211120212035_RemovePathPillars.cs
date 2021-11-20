using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Infrastructure.Persistance.Migrations
{
    public partial class RemovePathPillars : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningRessource_LearningPathPillar_LearningPathPillarId",
                table: "LearningRessource");

            migrationBuilder.DropTable(
                name: "LearningPathPillar");

            migrationBuilder.RenameColumn(
                name: "LearningPathPillarId",
                table: "LearningRessource",
                newName: "LearningPathId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningRessource_LearningPathPillarId",
                table: "LearningRessource",
                newName: "IX_LearningRessource_LearningPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningRessource_LearningPaths_LearningPathId",
                table: "LearningRessource",
                column: "LearningPathId",
                principalTable: "LearningPaths",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LearningRessource_LearningPaths_LearningPathId",
                table: "LearningRessource");

            migrationBuilder.RenameColumn(
                name: "LearningPathId",
                table: "LearningRessource",
                newName: "LearningPathPillarId");

            migrationBuilder.RenameIndex(
                name: "IX_LearningRessource_LearningPathId",
                table: "LearningRessource",
                newName: "IX_LearningRessource_LearningPathPillarId");

            migrationBuilder.CreateTable(
                name: "LearningPathPillar",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LearningPathId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LearningPathPillar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LearningPathPillar_LearningPaths_LearningPathId",
                        column: x => x.LearningPathId,
                        principalTable: "LearningPaths",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LearningPathPillar_LearningPathId",
                table: "LearningPathPillar",
                column: "LearningPathId");

            migrationBuilder.AddForeignKey(
                name: "FK_LearningRessource_LearningPathPillar_LearningPathPillarId",
                table: "LearningRessource",
                column: "LearningPathPillarId",
                principalTable: "LearningPathPillar",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
