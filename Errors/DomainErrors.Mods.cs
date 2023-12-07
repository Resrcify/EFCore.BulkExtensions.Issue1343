using EFCore.BulkExtensions.Issue1343.Shared;

namespace EFCore.BulkExtensions.Issue1343.Errors;

public static partial class DomainErrors
{
    public static class ModId
    {
        public static readonly Func<string, Error> GuidConvertionFailed = playerId => new Error(
            "ModId.GuidConvertionFailed",
            $"Unable to convert {playerId} to a Guid format.");
        public static readonly Func<string, int, Error> ToLong = (value, maxLength) => new Error(
            "ModId.ToLong",
            $"{value} exceeds the max length of {maxLength}.");
        public static readonly Error Empty = new(
            "ModId.Empty",
            $"Cannot be empty.");
        public static readonly Func<string, int, Error> ToShort = (value, minLength) => new Error(
            "ModId.ToShort",
            $"{value} is less than the min length of {minLength}.");
    }
    public static class ModDefinitionId
    {
        public static readonly Func<string, int, Error> ToLong = (value, maxLength) => new Error(
            "ModDefinitionId.ToLong",
            $"{value} exceeds the max length of {maxLength}.");
        public static readonly Error Empty = new(
            "ModDefinitionId.Empty",
            $"Cannot be empty.");
        public static readonly Func<string, int, Error> ToShort = (value, minLength) => new Error(
            "ModDefinitionId.ToShort",
            $"{value} is less than the min length of {minLength}.");
    }
    public static class Mod
    {
        public static readonly Func<string, string, Error> NotEqual = (oldValue, newValue) => new Error(
            "Mod.NotEqual",
            $"Id {oldValue} and Id {newValue} are not equal.");
    }
}