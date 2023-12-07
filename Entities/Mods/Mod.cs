using EFCore.BulkExtensions.Issue1343.Base;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.Enums;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;
using EFCore.BulkExtensions.Issue1343.Shared;
using EFCore.BulkExtensions.Issue1343.Errors;

namespace EFCore.BulkExtensions.Issue1343.Entities.Mods;

public sealed class Mod : Entity<ModId>
{
    public PlayerId PlayerId { get; private set; }
    public ModDefinitionId ModDefinitionId { get; private set; }
    public int Level { get; private set; }
    public IReadOnlyCollection<ModStat> Stats => _stats;
    private readonly List<ModStat> _stats = [];
    public ModTier ModTier { get; private set; }
    public ModSlot Slot { get; private set; }
    public ModType Type { get; private set; }
    public ModRarity Rarity { get; private set; }
    public int RerolledCount { get; private set; }

    private Mod(ModId id, PlayerId playerId, ModDefinitionId modDefinitionId, ModType type, ModRarity rarity, ModSlot slot, int level, ModTier modTier, int rerolledCount) : base(id)
    {
        PlayerId = playerId;
        ModDefinitionId = modDefinitionId;
        Level = level;
        ModTier = modTier;
        RerolledCount = rerolledCount;
        Type = type;
        Rarity = rarity;
        Slot = slot;
    }

    public static Result<Mod> Create(string id, PlayerId playerId, string modDefinitionId, ModType type, ModRarity rarity, ModSlot slot, int level, ModTier modTier, int rerolledCount, IEnumerable<ModStat> modStat)
    {
        var modId = ModId.Create(id);
        var mDefId = ModDefinitionId.Create(modDefinitionId);
        var mod = Result
            .Combine(modId, mDefId)
            .Map(id => new Mod(
                id.Item1,
                playerId,
                id.Item2,
                type,
                rarity,
                slot,
                level,
                modTier,
                rerolledCount));

        if (mod.IsFailure)
            return mod;

        mod.Value.AddStats(modStat);
        return mod;
    }

    private Result AddStats(IEnumerable<ModStat> modStats)
    {
        _stats.AddRange(modStats);
        return Result.Success();
    }

    public Result Update(Mod mod)
    {
        if (!Id.Equals(mod.Id))
            return DomainErrors.Mod.NotEqual(Id.Value, mod.Id.Value);
        if (!ModTier.Equals(mod.ModTier))
            ModTier = mod.ModTier;
        if (!ModDefinitionId.Equals(mod.ModDefinitionId))
            ModDefinitionId = mod.ModDefinitionId;
        if (!Level.Equals(mod.Level))
            Level = mod.Level;
        if (!RerolledCount.Equals(mod.RerolledCount))
            RerolledCount = mod.RerolledCount;

        if (!Stats.OrderBy(x => x.Position).SequenceEqual(mod.Stats.OrderBy(x => x.Position)))
        {
            _stats.Clear();
            _stats.AddRange(mod.Stats);
        }

        return Result.Success();
    }

    public string GetModSlotName()
        => (int)Slot switch
        {
            1 => "Square",
            2 => "Arrow",
            3 => "Diamond",
            4 => "Triangle",
            5 => "Circle",
            6 => "Cross",
            _ => "None"
        };
    public string GetModTypeName()
        => (int)Type switch
        {
            1 => "Health",
            2 => "Offence",
            3 => "Defence",
            4 => "Speed",
            5 => "Critical Chance",
            6 => "Critical Damage",
            7 => "Potency",
            8 => "Tenacity",
            _ => "None"
        };

    public string GetModTierName()
        => ModTier.ToString();
}