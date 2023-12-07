using EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.BulkExtensions.Issue1343.Configurations
{
    public class ModStatConfiguration : IEntityTypeConfiguration<ModStat>
    {
        public void Configure(EntityTypeBuilder<ModStat> builder)
        {
            builder
                .ToTable("ModStat");

            builder
                .HasKey(x => new { x.ModId, x.PlayerId, x.Position });

            builder
                .Property(x => x.ModId)
                .HasConversion(x => x.Value, v => ModId.Create(v).Value)
                .HasMaxLength(ModId.MaxLength)
                .ValueGeneratedNever()
                .IsRequired();

            builder
                .Property(x => x.PlayerId)
                .HasConversion(x => x.Value, v => PlayerId.Create(v).Value)
                .HasMaxLength(PlayerId.MaxLength)
                .ValueGeneratedNever()
                .IsRequired();

            builder
                .Property(x => x.Position)
                .IsRequired();

            builder
                .Property(x => x.StatType)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.IsPercentage)
                .IsRequired();

            builder
                .Property(x => x.Value)
                .HasPrecision(8, 2)
                .IsRequired();

            builder
                .Property(x => x.StatRolls)
                .IsRequired();
        }
    }
}