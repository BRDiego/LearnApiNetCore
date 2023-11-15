using ApiNetCore.Business.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options) { }

        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var property in modelBuilder.Model.GetEntityTypes()
                .SelectMany(e => e.GetProperties()
                    .Where(p => p.ClrType == typeof(DateOnly))))
                property.SetColumnType("date");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Band> Bands { get; set; }
        public DbSet<Musician> Musicians { get; set; }
    }
}