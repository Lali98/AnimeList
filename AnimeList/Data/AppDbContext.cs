using AnimeList.Models;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<FanSub> FanSubs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Anime> Animes { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

            modelBuilder.Entity<Anime>()
                .HasIndex(x => x.MalId)
                .IsUnique();

            modelBuilder.Entity<Genre>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Studio>()
                .HasIndex(x => x.Name)
                .IsUnique();

            modelBuilder.Entity<Anime>()
                .HasMany(x => x.Genres)
                .WithMany(x => x.Animes);

            modelBuilder.Entity<Anime>()
                .HasMany(x => x.Studios)
                .WithMany(x => x.Animes);
        }

        public override async Task<int> SaveChangesAsync (CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries<BaseEntity>();
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedAt = DateTime.UtcNow;
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
                else if (entry.State == EntityState.Modified)
                {
                    entry.Entity.UpdatedAt = DateTime.UtcNow;
                }
            }
            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
