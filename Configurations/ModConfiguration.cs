using EFCore.BulkExtensions.Issue1343.Entities.Mods;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EFCore.BulkExtensions.Issue1343.Configurations
{
    public class ModConfiguration : IEntityTypeConfiguration<Mod>
    {
        public void Configure(EntityTypeBuilder<Mod> builder)
        {
            builder
                .ToTable("Mods");

            builder
                .HasKey(x => new { x.Id, x.PlayerId });

            builder
                .Property(x => x.Id)
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
                .Property(x => x.ModDefinitionId)
                .HasConversion(x => x.Value, v => ModDefinitionId.Create(v).Value)
                .HasMaxLength(ModDefinitionId.MaxLength)
                .ValueGeneratedNever()
                .IsRequired();

            builder
                .Property(x => x.Slot)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.Type)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.Rarity)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.ModTier)
                .HasConversion<int>()
                .IsRequired();

            builder
                .Property(x => x.Level)
                .IsRequired();

            builder
                .Property(x => x.RerolledCount)
                .IsRequired();

            builder
                .HasMany(x => x.Stats)
                .WithOne();

            builder
                .Metadata
                .FindNavigation(nameof(Mod.Stats))!
                    .SetPropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}