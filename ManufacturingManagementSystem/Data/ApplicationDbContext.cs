using Microsoft.EntityFrameworkCore;
using ManufacturingManagementSystem.Models;

namespace ManufacturingManagementSystem.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Order> Orders { get; set; }
        public DbSet<Material> Materials { get; set; }
        public DbSet<ProductionLine> ProductionLines { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<OrderMaterial> OrderMaterials { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<OrderMaterial>()
                .HasKey(om => new { om.OrderId, om.MaterialId });

            modelBuilder.Entity<OrderMaterial>()
                .HasOne(om => om.Order)
                .WithMany(o => o.OrderMaterials)
                .HasForeignKey(om => om.OrderId);

            modelBuilder.Entity<OrderMaterial>()
                .HasOne(om => om.Material)
                .WithMany(m => m.OrderMaterials)
                .HasForeignKey(om => om.MaterialId);
        }
    }
}
