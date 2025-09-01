using EmployeeService.Core;
using Microsoft.EntityFrameworkCore;

namespace EmployeeService.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<Employee> Employees { get; set; }
    public DbSet<Manager> Managers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Department).IsRequired().HasMaxLength(50);
            entity.Property(e => e.Age).IsRequired();
            entity.Property(e => e.Salary).IsRequired();
            entity.Property(e => e.HireDate).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();

            // Self-referencing relationship for Manager
            entity.HasOne(e => e.Manager)
                  .WithMany(e => e.TeamMembers)
                  .HasForeignKey(e => e.ManagerId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Manager>(entity =>
        {
            entity.Property(e => e.ManagementLevel).HasMaxLength(50);
        });
    }
} 