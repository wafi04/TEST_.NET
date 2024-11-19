using Microsoft.EntityFrameworkCore;
using CrudApi.Models;

namespace CrudApi.Data;

public class ApplicationDbContext : DbContext 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) 
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Karyawan> Karyawan { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity => 
        {
            // Existing User configurations
            entity.HasIndex(u => u.Email).IsUnique();
            entity.ToTable("Users");
            entity.Property(u => u.Name).HasMaxLength(100).IsRequired();
            entity.Property(u => u.Email).HasMaxLength(255).IsRequired();
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Configure one-to-many relationship
            entity.HasMany(u => u.Karyawan)
                  .WithOne(k => k.User)
                  .HasForeignKey(k => k.UserId)
                  .OnDelete(DeleteBehavior.Restrict); // Prevent cascade delete
        });

        modelBuilder.Entity<Karyawan>(entity =>
        {
            entity.HasKey(k => k.Nik);
            
            // Configure relationship from Karyawan side
            entity.HasOne(k => k.User)
                  .WithMany(u => u.Karyawan)
                  .HasForeignKey(k => k.UserId)
                  .OnDelete(DeleteBehavior.Restrict);
        });
    }
}