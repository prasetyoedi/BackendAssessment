using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BackendAssessment.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Plannings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RequestCode = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CandidateToken = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Version = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plannings", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanningSlots",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanningId = table.Column<int>(type: "int", nullable: false),
                    SlotOrder = table.Column<int>(type: "int", nullable: false),
                    SlotName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OriginalQuantity = table.Column<int>(type: "int", nullable: false),
                    BalancedQuantity = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanningSlots", x => x.Id);
                    table.CheckConstraint("CK_BalancedQuantity_NonNegative", "BalancedQuantity >= 0");
                    table.CheckConstraint("CK_OriginalQuantity_NonNegative", "OriginalQuantity >= 0");
                    table.ForeignKey(
                        name: "FK_PlanningSlots_Plannings_PlanningId",
                        column: x => x.PlanningId,
                        principalTable: "Plannings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Plannings_RequestCode",
                table: "Plannings",
                column: "RequestCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlanningSlots_PlanningId",
                table: "PlanningSlots",
                column: "PlanningId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PlanningSlots");

            migrationBuilder.DropTable(
                name: "Plannings");
        }
    }
}
