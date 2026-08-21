using BackendAssessment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace BackendAssessment.Persistence.DbContext;

public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Planning> Plannings { get; set; }
    public DbSet<PlanningSlot> PlanningSlots { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Planning>()
            .HasIndex(p => p.RequestCode)
            .IsUnique();
        modelBuilder.Entity<PlanningSlot>()
            .ToTable(tb => tb.HasCheckConstraint("CK_OriginalQuantity_NonNegative", "OriginalQuantity >= 0"));
        modelBuilder.Entity<PlanningSlot>()
            .ToTable(tb => tb.HasCheckConstraint("CK_BalancedQuantity_NonNegative", "BalancedQuantity >= 0"));
        modelBuilder.Entity<PlanningSlot>()
            .HasOne(ps => ps.Planning)
            .WithMany(p => p.Slots)
            .HasForeignKey(ps => ps.PlanningId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}