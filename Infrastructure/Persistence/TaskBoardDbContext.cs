using Microsoft.EntityFrameworkCore;
using TaskBoard.Api.Domain;

namespace TaskBoard.Api.Infrastructure.Persistence;

public class TaskBoardDbContext : DbContext
{
    public TaskBoardDbContext(DbContextOptions<TaskBoardDbContext> options) : base(options)
    {
        
    }

    public DbSet<TaskItem> Tasks => Set<TaskItem>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<DateTime>().HaveConversion<UtcDateTimeConverter>();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TaskItem>(task =>
        {
            task.ToTable("tasks");
            task.HasKey(t => t.Id);
            task.Property(t => t.Title);
            task.Property(t => t.Title).HasMaxLength(TaskItem.MaxTitleLength).IsRequired();
            task.Property(t => t.Status).HasConversion<string>().HasMaxLength(20);
        });
    }
}