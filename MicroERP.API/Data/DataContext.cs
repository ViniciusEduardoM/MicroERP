using Microsoft.EntityFrameworkCore;
using MicroERP.ModelsDB.Models.Documents;
using MicroERP.ModelsDB.Models.MasterData;

namespace MicroERP.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderLine>().HasKey(s => new { s.DocumentId, s.Line });

        }

        public DbSet<Partner> Partners { get;set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Seller> Sellers { get; set; }

    }
}
