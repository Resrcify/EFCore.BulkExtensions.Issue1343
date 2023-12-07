using EFCore.BulkExtensions.Issue1343.Shared;

namespace EFCore.BulkExtensions.Issue1343.Errors;

public static partial class DomainErrors
{
    public static class Player
    {
        public static readonly Func<string, Error> SimulatedUnitAlreadyExist = Id => new Error(
            "Player.SimulatedUnitAlreadyExist",
            $"{Id} already exist.");
        public static readonly Func<string, Error> SimulatedUnitDoesntExist = Id => new Error(
            "Player.SimulatedUnitDoesntExist",
            $"{Id} doesn't exist.");
        public static readonly Func<string, Error> OpponentAlreadyExist = Id => new Error(
            "Player.OpponentAlreadyExist",
            $"{Id} already exist.");
        public static readonly Func<string, Error> OpponentDoesntExist = Id => new Error(
            "Player.OpponentDoesntExist",
            $"{Id} doesn't exist.");
    }
    public static class Opponent
    {
        public static readonly Func<string, Error> Identical = Id => new Error(
            "Opponent.Identical",
            $"Opponent id {Id} identical with the players id.");
    }
    public static class PlayerId
    {
        public static readonly Func<string, Error> GuidConvertionFailed = playerId => new Error(
            "PlayerId.GuidConvertionFailed",
            $"Unable to convert {playerId} to a Guid format.");
        public static readonly Func<string, int, Error> ToLong = (value, maxLength) => new Error(
            "PlayerId.ToLong",
            $"{value} exceeds the max length of {maxLength}.");
        public static readonly Error Empty = new(
            "PlayerId.Empty",
            $"Cannot be empty.");
        public static readonly Func<string, int, Error> ToShort = (value, minLength) => new Error(
            "PlayerId.ToShort",
            $"{value} is less than the min length of {minLength}.");
        public static readonly Func<string, string, Error> NotEqual = (value1, value2) => new Error(
            "PlayerId.NotEqual",
            $"{value1} is not equal to {value2}.");
    }

    public static class PlayerName
    {
        public static readonly Func<string, int, Error> ToLong = (value, maxLength) => new Error(
            "PlayerName.ToLong",
            $"{value} exceeds the max length of {maxLength}.");
        public static readonly Error Empty = new(
            "PlayerName.Empty",
            $"Cannot be empty.");
        public static readonly Func<string, int, Error> ToShort = (value, minLength) => new Error(
            "PlayerName.ToShort",
            $"{value} is less than the min length of {minLength}.");
    }
    public static class AllyCode
    {
        public static readonly Func<long, int, int, Error> ExccedsPermittedValues = (value, lowerLimit, upperLimit) => new Error(
            "AllyCode.ExccedsPermittedValues",
            $"{value} is not between {lowerLimit} and {upperLimit}.");
        public static readonly Error Empty = new(
            "AllyCode.Empty",
            $"Cannot be empty.");
    }
}