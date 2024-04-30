using Microsoft.EntityFrameworkCore;
using TopupService.DAL.Entities;

namespace TopupService.DAL
{
    public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
    {
        public DbSet<BeneficiaryEntity> Beneficiaries { get; set; }
        public DbSet<TopupHistoryEntity> TopupHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TopupHistoryEntity>()
            .Property(t => t.TopupAmount)
            .HasColumnType("decimal(18, 2)");
        }
    }
}
