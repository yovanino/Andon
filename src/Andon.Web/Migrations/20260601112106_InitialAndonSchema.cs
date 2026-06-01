using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Andon.Web.Migrations
{
    /// <inheritdoc />
    public partial class InitialAndonSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "operational_events",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    Source = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    EventType = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    Severity = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    OccurredUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    MachineId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    MachineName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    LineCode = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    WorkOrderId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    ExternalProjectId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ExternalTaskId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ExternalEventId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    SourceSystem = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    PayloadJson = table.Column<string>(type: "longtext", nullable: false),
                    TaskSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    ContextSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    CreatedByPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    CreatedByPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_operational_events", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "andon_alerts",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    OperationalEventId = table.Column<long>(type: "bigint", nullable: true),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Severity = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    MachineId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    MachineName = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    LineCode = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    WorkOrderId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    ExternalProjectId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ExternalTaskId = table.Column<Guid>(type: "char(36)", nullable: true),
                    SourceSystem = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    TaskSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    ContextSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    ExternalRcaIncidentId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    RcaStatus = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    RcaCreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResponsiblePrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    ResponsiblePrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    ResponsibleDisplayName = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    OpenedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    AcknowledgedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    AssignedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    EscalatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResolvedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ResolutionComment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    CreatedByPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    CreatedByPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_andon_alerts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_andon_alerts_operational_events_OperationalEventId",
                        column: x => x.OperationalEventId,
                        principalTable: "operational_events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "action_items",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    RelatedAndonAlertId = table.Column<long>(type: "bigint", nullable: true),
                    ExternalRcaIncidentId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    ExternalProjectId = table.Column<Guid>(type: "char(36)", nullable: true),
                    ExternalTaskId = table.Column<Guid>(type: "char(36)", nullable: true),
                    SourceSystem = table.Column<string>(type: "varchar(40)", maxLength: 40, nullable: false),
                    TaskSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    ContextSnapshotJson = table.Column<string>(type: "longtext", nullable: false),
                    Title = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    Description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    Status = table.Column<string>(type: "varchar(32)", maxLength: 32, nullable: false),
                    Priority = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    AssignedPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    AssignedPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    AssignedDisplayName = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    DueDate = table.Column<DateOnly>(type: "date", nullable: true),
                    ClosedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ClosureComment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    CreatedByPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    CreatedByPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_action_items", x => x.Id);
                    table.ForeignKey(
                        name: "FK_action_items_andon_alerts_RelatedAndonAlertId",
                        column: x => x.RelatedAndonAlertId,
                        principalTable: "andon_alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "andon_comments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    TenantId = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: false),
                    AndonAlertId = table.Column<long>(type: "bigint", nullable: false),
                    Comment = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    CreatedByPrincipalType = table.Column<string>(type: "varchar(24)", maxLength: 24, nullable: false),
                    CreatedByPrincipalId = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    CreatedByDisplayName = table.Column<string>(type: "varchar(160)", maxLength: 160, nullable: false),
                    CreatedUtc = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_andon_comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_andon_comments_andon_alerts_AndonAlertId",
                        column: x => x.AndonAlertId,
                        principalTable: "andon_alerts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_action_items_RelatedAndonAlertId",
                table: "action_items",
                column: "RelatedAndonAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_action_items_TenantId_AssignedPrincipalType_AssignedPrincipa~",
                table: "action_items",
                columns: new[] { "TenantId", "AssignedPrincipalType", "AssignedPrincipalId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_action_items_TenantId_ExternalRcaIncidentId",
                table: "action_items",
                columns: new[] { "TenantId", "ExternalRcaIncidentId" });

            migrationBuilder.CreateIndex(
                name: "IX_action_items_TenantId_ExternalTaskId_Status",
                table: "action_items",
                columns: new[] { "TenantId", "ExternalTaskId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_action_items_TenantId_RelatedAndonAlertId",
                table: "action_items",
                columns: new[] { "TenantId", "RelatedAndonAlertId" });

            migrationBuilder.CreateIndex(
                name: "IX_action_items_TenantId_Status_DueDate",
                table: "action_items",
                columns: new[] { "TenantId", "Status", "DueDate" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_OperationalEventId",
                table: "andon_alerts",
                column: "OperationalEventId");

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_TenantId_ExternalRcaIncidentId",
                table: "andon_alerts",
                columns: new[] { "TenantId", "ExternalRcaIncidentId" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_TenantId_ExternalTaskId_Status",
                table: "andon_alerts",
                columns: new[] { "TenantId", "ExternalTaskId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_TenantId_MachineId_Status",
                table: "andon_alerts",
                columns: new[] { "TenantId", "MachineId", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_TenantId_Severity_Status",
                table: "andon_alerts",
                columns: new[] { "TenantId", "Severity", "Status" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_alerts_TenantId_Status_OpenedUtc",
                table: "andon_alerts",
                columns: new[] { "TenantId", "Status", "OpenedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_andon_comments_AndonAlertId",
                table: "andon_comments",
                column: "AndonAlertId");

            migrationBuilder.CreateIndex(
                name: "IX_andon_comments_TenantId_AndonAlertId_CreatedUtc",
                table: "andon_comments",
                columns: new[] { "TenantId", "AndonAlertId", "CreatedUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_events_TenantId_ExternalTaskId",
                table: "operational_events",
                columns: new[] { "TenantId", "ExternalTaskId" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_events_TenantId_MachineId_OccurredUtc",
                table: "operational_events",
                columns: new[] { "TenantId", "MachineId", "OccurredUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_events_TenantId_OccurredUtc",
                table: "operational_events",
                columns: new[] { "TenantId", "OccurredUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_events_TenantId_Severity_OccurredUtc",
                table: "operational_events",
                columns: new[] { "TenantId", "Severity", "OccurredUtc" });

            migrationBuilder.CreateIndex(
                name: "IX_operational_events_TenantId_Source_EventType",
                table: "operational_events",
                columns: new[] { "TenantId", "Source", "EventType" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "action_items");

            migrationBuilder.DropTable(
                name: "andon_comments");

            migrationBuilder.DropTable(
                name: "andon_alerts");

            migrationBuilder.DropTable(
                name: "operational_events");
        }
    }
}
