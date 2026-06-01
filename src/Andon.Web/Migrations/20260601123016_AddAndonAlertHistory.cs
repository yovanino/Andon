using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Andon.Web.Migrations
{
    /// <inheritdoc />
    public partial class AddAndonAlertHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "andon_alert_history",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    AndonAlertId = table.Column<long>(type: "bigint", nullable: false),
                    Action = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    FromStatus = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    ToStatus = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: true),
                    Comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    ActorPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    ActorPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_andon_alert_history", x => x.Id);
                    table.ForeignKey(
                        name: "FK_andon_alert_history_andon_alerts_AndonAlertId",
                        column: x => x.AndonAlertId,
                        principalTable: "andon_alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_andon_alert_history_AndonAlertId",
                table: "andon_alert_history",
                column: "AndonAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_andon_alert_history_TenantId_Action_CreatedUtc",
                table: "andon_alert_history",
                columns: new[] { "TenantId", "Action", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alert_history_TenantId_AndonAlertId_CreatedUtc",
                table: "andon_alert_history",
                columns: new[] { "TenantId", "AndonAlertId", "CreatedUtc" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "andon_alert_history");
        }
    }
}
