using Microsoft.EntityFrameworkCore;

namespace WarehouseTestService.Data
{
    public class WarehouseContext : DbContext
    {
        public DbSet<BoxModel> Boxes { get; set; }
        public DbSet<PalletModel> Pallets { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=warehouse.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BoxModel>()
                .HasOne(b => b.Pallet)
                .WithMany(p => p.Boxes)
                .HasForeignKey(b => b.PalletId);
        }
    }
}
