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
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(1000);

            builder.ComplexProperty(x => x.Links)
                .ToJson();
        }
    }
}
