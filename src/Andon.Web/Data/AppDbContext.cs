using Andon.Web.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace Andon.Web.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<OperationalEvent> OperationalEvents => Set<OperationalEvent>();

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
    }
}
