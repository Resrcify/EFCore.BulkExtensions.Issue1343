using EFCore.BulkExtensions.Issue1343.Shared;
using EFCore.BulkExtensions.Issue1343.Base;
using EFCore.BulkExtensions.Issue1343.Errors;

namespace EFCore.BulkExtensions.Issue1343.Entities.Mods.ValueObjects;

public sealed class ModId : ValueObject
{
    public const int MaxLength = 50;
    public const int MinLength = 1;
    public string Value;
    private ModId(string value)
        => Value = value;

    public static Result<ModId> Create(string value)
        => Result.Ensure(
            value,
            (v => !string.IsNullOrEmpty(v), DomainErrors.ModId.Empty),
            (v => v.Length <= MaxLength, DomainErrors.ModId.ToLong(value, MaxLength)),
            (v => v.Length >= MinLength, DomainErrors.ModId.ToShort(value, MinLength)),
            (CanConvertToGuid, DomainErrors.ModId.GuidConvertionFailed(value)))
            .Map(e => new ModId(e));

    public override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }

    public static Result<Guid> ToGuidId(string arg)
    {
        var convertResult = Base64UrlDecode(arg);
        if (convertResult.IsFailure)
            return Result.Failure<Guid>(convertResult.Errors);

        return new Guid(convertResult.Value);
    }

    public static Result<string> ToStringId(Guid arg)
        => Base64UrlEncode(arg.ToByteArray());

    private static string Base64UrlEncode(byte[] arg)
    {
        string s = Convert.ToBase64String(arg); // Regular base64 encoder
        s = s.Split('=')[0]; // Remove any trailing '='s
        s = s.Replace('+', '-'); // 62nd char of encoding
        s = s.Replace('/', '_'); // 63rd char of encoding
        return s;
    }
    private static bool CanConvertToGuid(string arg)
        => Base64UrlDecode(arg).IsSuccess;

    private static Result<byte[]> Base64UrlDecode(string arg)
    {
        if (string.IsNullOrWhiteSpace(arg))
            return Result.Failure<byte[]>(DomainErrors.PlayerId.GuidConvertionFailed(arg));

        string s = arg;
        s = s.Replace('-', '+'); // 62nd char of encoding
        s = s.Replace('_', '/'); // 63rd char of encoding
        switch (s.Length % 4) // Pad with trailing '='s
        {
            case 0: break; // No pad chars in this case
            case 2: s += "=="; break; // Two pad chars
            case 3: s += "="; break; // One pad char
            default:
                return Result.Failure<byte[]>(DomainErrors.PlayerId.GuidConvertionFailed(arg));
        }

        Span<byte> buffer = new(new byte[s.Length]);

        if (!Convert.TryFromBase64String(s, buffer, out int bytesParsed))
            return Result.Failure<byte[]>(DomainErrors.PlayerId.GuidConvertionFailed(arg));

        return buffer.ToArray(); // Standard base64 decoder
    }
}