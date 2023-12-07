using System;

namespace EFCore.BulkExtensions.Issue1343.Base;
public abstract class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; private init; }

    protected Entity(TId id)
    {
        Id = id;
    }

    public static bool operator ==(Entity<TId> first, Entity<TId> second)
    {
        return first is not null && second is not null && first.Equals(second);
    }

    public static Guid ToGuidId(string arg)
    {
        return new Guid(Base64UrlDecode(arg));
    }

    public static string ToStringId(Guid arg)
    {
        return Base64UrlEncode(arg.ToByteArray());
    }

    private static string Base64UrlEncode(byte[] arg)
    {
        string s = Convert.ToBase64String(arg); // Regular base64 encoder
        s = s.Split('=')[0]; // Remove any trailing '='s
        s = s.Replace('+', '-'); // 62nd char of encoding
        s = s.Replace('/', '_'); // 63rd char of encoding
        return s;
    }

    private static byte[] Base64UrlDecode(string arg)
    {
        string s = arg;
        s = s.Replace('-', '+'); // 62nd char of encoding
        s = s.Replace('_', '/'); // 63rd char of encoding
        switch (s.Length % 4) // Pad with trailing '='s
        {
            case 0: break; // No pad chars in this case
            case 2: s += "=="; break; // Two pad chars
            case 3: s += "="; break; // One pad char
            default:
                throw new Exception(
              "Illegal base64url string!");
        }
        return Convert.FromBase64String(s); // Standard base64 decoder
    }

    public static bool operator !=(Entity<TId> first, Entity<TId> second)
    {
        return !(first == second);
    }

    public override bool Equals(object? obj)
    {
        if (obj is null) return false;
        if (obj.GetType() != GetType()) return false;
        if (obj is not Entity<TId> entity) return false;
        return Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode() * 41;
    }

    public bool Equals(Entity<TId>? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;
        return Id.Equals(other.Id);
    }
}