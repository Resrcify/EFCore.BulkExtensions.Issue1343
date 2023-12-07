using EFCore.BulkExtensions.Issue1343.Shared;
using EFCore.BulkExtensions.Issue1343.Base;

namespace EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;

public sealed class ModStat : ValueObject
{
    public ModId ModId { get; init; }
    public PlayerId PlayerId { get; init; }
    public StatType StatType { get; init; }
    public int Position { get; init; }
    public float Value { get; init; }
    public int StatRolls { get; init; }
    public bool IsPercentage { get; init; }

    private ModStat(ModId modId, PlayerId playerId, StatType statType, int position, float value, int statRolls, bool isPercentage)
    {
        ModId = modId;
        PlayerId = playerId;
        StatType = statType;
        Position = position;
        Value = value;
        StatRolls = statRolls;
        IsPercentage = isPercentage;
    }

    public static Result<ModStat> Create(string modId, PlayerId playerId, StatType statType, int statPosition, double value, int statRolls, bool isPercentage)
    {
        var mId = ModId.Create(modId);
        return Result
            .Combine(mId)
            .Map(p => new ModStat(p, playerId, statType, statPosition, (float)Math.Round(value, 2), statRolls, isPercentage));
    }

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return ModId;
        yield return PlayerId;
        yield return Position;
        yield return StatType;
        yield return Value;
        yield return StatRolls;
        yield return IsPercentage;
    }
}