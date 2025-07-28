using ErrorOr;
    using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Domain.Users.ValueObjects;

public sealed class Password : IEquatable<Password>
{
    private const int MinLength = 8;
    private const int SaltSize = 16;
    private const int KeySize = 32;
    private const int Iterations = 100_000;
    private const string HashPrefix = "PBKDF2$";
    private static readonly Regex PatternNumber = new(@"\d", RegexOptions.Compiled);
    private static readonly Regex PatternUpperChar = new(@"[A-Z]", RegexOptions.Compiled);
    private static readonly Regex PatternHasLowerChar = new(@"[a-z]", RegexOptions.Compiled);
    private static readonly Regex PatternHasSpecialChar = new(@"[\W_]", RegexOptions.Compiled);

    public string Value { get; }

    public Password(string value)
    {
        Value = value;
    }

    public static ErrorOr<Password> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Password cannot be empty.");

        if (value.StartsWith(HashPrefix))
            return new Password(value);

        if (value.Length < MinLength)
            return Error.Validation(nameof(value), $"Password must be at least {MinLength} characters long.");
        
        if (!PatternNumber.IsMatch(value))
            return Error.Validation(nameof(value), "Password must contain at least one number.");
        
        if (!PatternUpperChar.IsMatch(value))
            return Error.Validation(nameof(value), "Password must contain at least one uppercase letter.");
        
        if (!PatternHasLowerChar.IsMatch(value))
            return Error.Validation(nameof(value), "Password must contain at least one lowercase letter.");
        
        if (!PatternHasSpecialChar.IsMatch(value))
            return Error.Validation(nameof(value), "Password must contain at least one special character.");

        return new Password(Hash(value));
    }

    public static ErrorOr<Password> CreateByLogin(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Password cannot be empty.");

        if (value.Length < MinLength)
            return Error.Validation(nameof(value), "Password invalid.");

        return new Password(value);
    }

    public static string Hash(string password)
    {
        using var rng = RandomNumberGenerator.Create();
        var salt = new byte[SaltSize];
        rng.GetBytes(salt);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, Iterations, HashAlgorithmName.SHA256);
        var hash = pbkdf2.GetBytes(KeySize);

        return $"{HashPrefix}{Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
    }

    public bool Verify(string plainPassword)
    {
        if (!Value.StartsWith(HashPrefix))
            throw new InvalidOperationException("Stored password is not hashed.");

        var parts = Value.Split('$');
        if (parts.Length != 4)
            throw new FormatException("Invalid hash format.");

        var iterations = int.Parse(parts[1]);
        var salt = Convert.FromBase64String(parts[2]);
        var hash = Convert.FromBase64String(parts[3]);

        using var pbkdf2 = new Rfc2898DeriveBytes(plainPassword, salt, iterations, HashAlgorithmName.SHA256);
        var hashToCompare = pbkdf2.GetBytes(KeySize);

        return CryptographicOperations.FixedTimeEquals(hash, hashToCompare);
    }

    public override bool Equals(object? obj) => Equals(obj as Password);

    public bool Equals(Password? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => "********";
}