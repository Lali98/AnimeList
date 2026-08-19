using AnimeList.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AnimeList.Data
{
    public class AppDbContext(
        DbContextOptions<AppDbContext> options
    ) : IdentityDbContext<ApplicationUser>(options)
    {
        public DbSet<FanSub> FanSubs { get; set; }
        public DbSet<Genre> Genres { get; set; }
        public DbSet<Studio> Studios { get; set; }
        public DbSet<Anime> Animes { get; set; }
        public DbSet<FanSubMember> FanSubMembers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(
                typeof(AppDbContext).Assembly);

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

            modelBuilder.Entity<FanSubMember>()
                .HasKey(x => new
                {
                    x.FanSubId,
                    x.UserId
                });

            modelBuilder.Entity<FanSubMember>()
                .HasOne(x => x.FanSub)
                .WithMany(x => x.Members)
                .HasForeignKey(x => x.FanSubId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FanSubMember>()
                .HasOne(x => x.User)
                .WithMany(x => x.FanSubs)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<FanSubMember>()
                .Property(x => x.Role)
                .HasConversion<string>();
        }

        public override async Task<int> SaveChangesAsync(
            CancellationToken cancellationToken = default)
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