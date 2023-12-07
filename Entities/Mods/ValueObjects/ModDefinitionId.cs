using EFCore.BulkExtensions.Issue1343.Shared;
using EFCore.BulkExtensions.Issue1343.Base;
using EFCore.BulkExtensions.Issue1343.Errors;
using EFCore.BulkExtensions.Issue1343.Entities.Mods.Enums;

namespace EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;

public sealed class ModDefinitionId : ValueObject
{
    public const int MaxLength = 3;
    public const int MinLength = 3;
    public string Value;
    private ModDefinitionId(string value)
        => Value = value;

    public static Result<ModDefinitionId> Create(string value)
        => Result.Ensure(
            value,
            (v => !string.IsNullOrEmpty(v), DomainErrors.ModDefinitionId.Empty),
            (v => v.Length <= MaxLength, DomainErrors.ModDefinitionId.ToLong(value, MaxLength)),
            (v => v.Length >= MinLength, DomainErrors.ModDefinitionId.ToShort(value, MinLength)))
            .Map(e => new ModDefinitionId(e));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public ModType GetSetId()
    {
        var defIdArray = Value.Select(digit => int.Parse(digit.ToString())).ToArray();
        return (ModType)defIdArray[0];
    }
    public ModRarity GetRarity()
    {
        var defIdArray = Value.Select(digit => int.Parse(digit.ToString())).ToArray();
        return (ModRarity)defIdArray[1];
    }
    public ModSlot GetSlot()
    {
        var defIdArray = Value.Select(digit => int.Parse(digit.ToString())).ToArray();
        return (ModSlot)defIdArray[2];
    }
}