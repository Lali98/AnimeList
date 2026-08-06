using AnimeList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeList.Data.Configurations
{
    public class AnimeConfiguration : IEntityTypeConfiguration<Anime>
    {
        public void Configure(EntityTypeBuilder<Anime> builder)
        {
            builder.HasIndex(x => x.MalId)
                .IsUnique();

            builder.ComplexProperty(x => x.Titles)
                .ToJson();

            builder.ComplexProperty(x => x.Descriptions)
                .ToJson();

            builder.Property(x => x.Type)
                .HasConversion<string>();

            builder.Property(x => x.Season)
                .HasConversion<string>();

            //builder.HasOne(x => x.FanSub)
            //    .WithMany(x => x.Animes)
            //    .HasForeignKey(x => x.FanSubId)
            //    .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
