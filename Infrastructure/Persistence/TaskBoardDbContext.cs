using System.Data;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Domain;
using TaskBoard.Api.Services;

namespace TaskBoard.Api.Infrastructure.Persistence;

public class TaskBoardDbContext : DbContext
{
    private const string VersionProperty = "Version";
    public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> options) : base(options)
    {
        
    }

    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Project>(project =>
        {
            project.ToTable("projects");
            project.HasKey(p => p.Id);
            project.Property(p => p.Name).HasMaxLength(Project.MaxNameLength).IsRequired();
            project.Property<Guid>(VersionProperty).IsConcurrencyToken();
        });

        modelBuilder.Entity<TaskItem>(task =>
        {
            task.ToTable("tasks");
            task.HasKey(t => t.Id);
            task.Property(t => t.Title);
            task.Property(t => t.Title).HasMaxLength(TaskItem.MaxTitleLength).IsRequired();
            task.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
            task.Property<Guid>(VersionProperty).IsConcurrencyToken();

            task.HasOne<Project>().WithMany().HasForeignKey(t => t.ProjectId).OnDelete(DeleteBehavior.Cascade);

            task.HasIndex(t => new { t.ProjectId, t.Status, t.CreatedAt });
        });
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Property(VersionProperty).CurrentValue = Guid.NewGuid();
            }
        }

        try
        {
            return await base.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new ConcurrencyConflictException("Dane zostaly w miedzyczasie zmienione przez kogos innego. Sproboj ponownie.", ex);
        }
    }
}