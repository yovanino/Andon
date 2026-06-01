using Andon.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Andon.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OperationalEvent> OperationalEvents => Set<OperationalEvent>();

    public DbSet<AndonAlert> AndonAlerts => Set<AndonAlert>();

    public DbSet<AndonComment> AndonComments => Set<AndonComment>();

    public DbSet<ActionItem> ActionItems => Set<ActionItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<OperationalEvent>(entity =>
        {
            entity.ToTable("operational_events");

            entity.HasKey(item => item.Id);
            entity.Property(item => item.TenantId).HasMaxLength(64).IsRequired();
            entity.Property(item => item.Source).HasConversion<string>().HasMaxLength(40).IsRequired();
            entity.Property(item => item.EventType).HasConversion<string>().HasMaxLength(60).IsRequired();
            entity.Property(item => item.Severity).HasConversion<string>().HasMaxLength(24).IsRequired();
            entity.Property(item => item.Title).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(2000);
            entity.Property(item => item.OccurredUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.MachineId).HasMaxLength(80);
            entity.Property(item => item.MachineName).HasMaxLength(80);
            entity.Property(item => item.LineCode).HasMaxLength(80);
            entity.Property(item => item.WorkOrderId).HasMaxLength(80);
            entity.Property(item => item.ExternalEventId).HasMaxLength(80);
            entity.Property(item => item.SourceSystem).HasMaxLength(40);
            entity.Property(item => item.PayloadJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.TaskSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.ContextSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.CreatedByPrincipalType).HasMaxLength(24).IsRequired();
            entity.Property(item => item.CreatedByPrincipalId).HasMaxLength(80);
            entity.Property(item => item.CreatedUtc).HasColumnType("datetime(6)");

            entity.HasIndex(item => new { item.TenantId, item.OccurredUtc });
            entity.HasIndex(item => new { item.TenantId, item.MachineId, item.OccurredUtc });
            entity.HasIndex(item => new { item.TenantId, item.ExternalTaskId });
            entity.HasIndex(item => new { item.TenantId, item.Severity, item.OccurredUtc });
            entity.HasIndex(item => new { item.TenantId, item.Source, item.EventType });
        });

        modelBuilder.Entity<AndonAlert>(entity =>
        {
            entity.ToTable("andon_alerts");

            entity.HasKey(item => item.Id);
            entity.Property(item => item.TenantId).HasMaxLength(64).IsRequired();
            entity.Property(item => item.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.Property(item => item.Severity).HasConversion<string>().HasMaxLength(24).IsRequired();
            entity.Property(item => item.Title).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(2000);
            entity.Property(item => item.MachineId).HasMaxLength(80);
            entity.Property(item => item.MachineName).HasMaxLength(80);
            entity.Property(item => item.LineCode).HasMaxLength(80);
            entity.Property(item => item.WorkOrderId).HasMaxLength(80);
            entity.Property(item => item.SourceSystem).HasMaxLength(40);
            entity.Property(item => item.TaskSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.ContextSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.ResponsiblePrincipalType).HasMaxLength(24);
            entity.Property(item => item.ResponsiblePrincipalId).HasMaxLength(80);
            entity.Property(item => item.ResponsibleDisplayName).HasMaxLength(160);
            entity.Property(item => item.OpenedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.AcknowledgedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.AssignedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.EscalatedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.ResolvedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.ResolutionComment).HasMaxLength(2000);
            entity.Property(item => item.CreatedByPrincipalType).HasMaxLength(24).IsRequired();
            entity.Property(item => item.CreatedByPrincipalId).HasMaxLength(80);
            entity.Property(item => item.CreatedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.UpdatedUtc).HasColumnType("datetime(6)");

            entity.HasOne(item => item.OperationalEvent)
                .WithMany(item => item.AndonAlerts)
                .HasForeignKey(item => item.OperationalEventId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(item => new { item.TenantId, item.Status, item.OpenedUtc });
            entity.HasIndex(item => new { item.TenantId, item.Severity, item.Status });
            entity.HasIndex(item => new { item.TenantId, item.ExternalTaskId, item.Status });
            entity.HasIndex(item => new { item.TenantId, item.MachineId, item.Status });
            entity.HasIndex(item => item.OperationalEventId);
        });

        modelBuilder.Entity<AndonComment>(entity =>
        {
            entity.ToTable("andon_comments");

            entity.HasKey(item => item.Id);
            entity.Property(item => item.TenantId).HasMaxLength(64).IsRequired();
            entity.Property(item => item.Comment).HasMaxLength(2000).IsRequired();
            entity.Property(item => item.CreatedByPrincipalType).HasMaxLength(24).IsRequired();
            entity.Property(item => item.CreatedByPrincipalId).HasMaxLength(80);
            entity.Property(item => item.CreatedByDisplayName).HasMaxLength(160);
            entity.Property(item => item.CreatedUtc).HasColumnType("datetime(6)");

            entity.HasOne(item => item.AndonAlert)
                .WithMany(item => item.Comments)
                .HasForeignKey(item => item.AndonAlertId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasIndex(item => new { item.TenantId, item.AndonAlertId, item.CreatedUtc });
        });

        modelBuilder.Entity<ActionItem>(entity =>
        {
            entity.ToTable("action_items");

            entity.HasKey(item => item.Id);
            entity.Property(item => item.TenantId).HasMaxLength(64).IsRequired();
            entity.Property(item => item.ExternalRcaIncidentId).HasMaxLength(80);
            entity.Property(item => item.SourceSystem).HasMaxLength(40);
            entity.Property(item => item.TaskSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.ContextSnapshotJson).HasColumnType("longtext").IsRequired();
            entity.Property(item => item.Title).HasMaxLength(200).IsRequired();
            entity.Property(item => item.Description).HasMaxLength(2000);
            entity.Property(item => item.Status).HasConversion<string>().HasMaxLength(32).IsRequired();
            entity.Property(item => item.Priority).HasConversion<string>().HasMaxLength(24).IsRequired();
            entity.Property(item => item.AssignedPrincipalType).HasMaxLength(24);
            entity.Property(item => item.AssignedPrincipalId).HasMaxLength(80);
            entity.Property(item => item.AssignedDisplayName).HasMaxLength(160);
            entity.Property(item => item.ClosureComment).HasMaxLength(2000);
            entity.Property(item => item.CreatedByPrincipalType).HasMaxLength(24).IsRequired();
            entity.Property(item => item.CreatedByPrincipalId).HasMaxLength(80);
            entity.Property(item => item.CreatedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.UpdatedUtc).HasColumnType("datetime(6)");
            entity.Property(item => item.ClosedUtc).HasColumnType("datetime(6)");

            entity.HasOne(item => item.RelatedAndonAlert)
                .WithMany(item => item.ActionItems)
                .HasForeignKey(item => item.RelatedAndonAlertId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasIndex(item => new { item.TenantId, item.Status, item.DueDate });
            entity.HasIndex(item => new { item.TenantId, item.ExternalTaskId, item.Status });
            entity.HasIndex(item => new { item.TenantId, item.ExternalRcaIncidentId });
            entity.HasIndex(item => new { item.TenantId, item.RelatedAndonAlertId });
            entity.HasIndex(item => new { item.TenantId, item.AssignedPrincipalType, item.AssignedPrincipalId, item.Status });
        });
    }
}
