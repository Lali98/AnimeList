using AnimeList.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace AnimeList.Data.Configurations
{
    public class FanSubConfiguration : IEntityTypeConfiguration<FanSub>
    {
        public void Configure(
            EntityTypeBuilder<FanSub> builder)
        {
            builder.Property(x => x.Name)
                .HasMaxLength(500)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasColumnType("text");

            builder.ComplexProperty(x => x.Links)
                .ToJson();
        }
    }
}
