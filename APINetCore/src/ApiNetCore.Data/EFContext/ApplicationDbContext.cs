using ApiNetCore.Business.Models;
using ApiNetCore.Business.Models.ManyToMany;
using Microsoft.EntityFrameworkCore;

namespace ApiNetCore.Data.EFContext
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        
        public DbSet<Band> Band { get; set; }
        public DbSet<Musician> Musician { get; set; }
        public DbSet<BandMusician> BandMusician { get; set; } // DbSet for the junction table

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure many-to-many relationship between Band and Musician using the junction table
            modelBuilder.Entity<BandMusician>()
                .HasKey(bm => new { bm.BandId, bm.MusicianId });

            modelBuilder.Entity<BandMusician>()
                .HasOne(bm => bm.Band)
                .WithMany()
                .HasForeignKey(bm => bm.BandId);

            modelBuilder.Entity<BandMusician>()
                .HasOne(bm => bm.Musician)
                .WithMany()
                .HasForeignKey(bm => bm.MusicianId);
        }
    }
}