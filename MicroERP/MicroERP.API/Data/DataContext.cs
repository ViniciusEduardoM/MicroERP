using Microsoft.EntityFrameworkCore;
using MicroERP.ModelsDB.Models.Documents;
using MicroERP.ModelsDB.Models.MasterData;
using MicroERP.ModelsDB.Models.MasterData.Users;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Options;
using MicroERP.API.Factory;
using MicroERP.API.Models.InternalDBTables;

namespace MicroERP.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base (options)
        {
            bool dataBaseExists = (Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator).Exists();

            if (dataBaseExists)
                Database.Migrate();
            else
                throw new Exception("Banco de dados informado não existe");

            new DefaultValuesFactory(this);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().ToTable("Orders");
            modelBuilder.Entity<OrderLine>().HasKey(s => new { s.DocumentId, s.Line });

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
        }

        public DbSet<Partner> Partners { get;set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Item> Items { get; set; }
        public DbSet<Seller> Sellers { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Role> Roles { get; set; }            
        public DbSet<PasswordRecory> PasswordRecovery { get;set; }

    }

    internal static class DbContextFactory
    {
        internal static string DefaultConnectionString { get; set; } = string.Empty;

        internal static DataContext CreateWithAuth(string? token)
        {
            string connStr = DefaultConnectionString + $"Database={ReadJWTGetCompanyDB(token)};";

            if (!string.IsNullOrEmpty(connStr))
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(connStr);
                return new DataContext(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException("ConnectionId");
            }
        }

        internal static DataContext CreateWithCompany(string? CompanyDB)
        {
            string connStr = DefaultConnectionString + $"Database={CompanyDB};";

            if (!string.IsNullOrEmpty(connStr))
            {
                var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
                optionsBuilder.UseSqlServer(connStr);
                return new DataContext(optionsBuilder.Options);
            }
            else
            {
                throw new ArgumentNullException("ConnectionId");
            }
        }

        private static string ReadJWTGetCompanyDB(string token)
        {
            JwtSecurityToken jwt = new JwtSecurityToken(token);

            string? CompanyDB = jwt.Claims?.FirstOrDefault(x => x.Type == "CompanyDB")?.Value;

            if (CompanyDB == null)
                throw new Exception("CompanyDB está ausente na autenticação");

            return CompanyDB;
        }
    }
}
