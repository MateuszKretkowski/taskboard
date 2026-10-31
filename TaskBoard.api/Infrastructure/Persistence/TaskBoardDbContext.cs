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

    public DbSet<ProjectMember> ProjectMembers => Set<ProjectMember>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ProjectMember>(pm =>
        {
            pm.ToTable("project_members");
            pm.HasKey(p => p.Id);
            pm.HasOne<User>().WithMany().HasForeignKey(p => p.UserId).OnDelete(DeleteBehavior.Cascade);
            pm.Property(p => p.ProjectId);
            pm.Property(p => p.IsOwner);
            pm.Property<Guid>(VersionProperty).IsConcurrencyToken();

            pm.HasIndex(p => new { p.ProjectId, p.UserId }).IsUnique();
        });
        modelBuilder.Entity<User>(user =>
        {
            user.ToTable("users");
            user.HasKey(u => u.Id);
            user.Property(u => u.Email).HasConversion(new StringEmailConverter()).HasMaxLength(Email.MaxValueLength).IsRequired();
            user.HasIndex(u => u.Email).IsUnique();
            user.Property(u => u.CreatedAt);
            user.Property(u => u.PasswordHash).IsRequired();
            user.Property<Guid>(VersionProperty).IsConcurrencyToken();
        });

        modelBuilder.Entity<Project>(project =>
        {
            project.ToTable("projects");    
            project.HasMany(p => p.Members).WithOne().HasForeignKey(p => p.ProjectId).OnDelete(DeleteBehavior.Cascade);
            project.HasKey(p => p.Id);
            project.Property(p => p.Name).HasMaxLength(Project.MaxNameLength).IsRequired();
            project.Property<Guid>(VersionProperty).IsConcurrencyToken();

            project.HasIndex(p => new { p.CreatedAt });
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