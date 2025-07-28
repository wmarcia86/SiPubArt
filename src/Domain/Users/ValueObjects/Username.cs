using ErrorOr;
using System.Text.RegularExpressions;

namespace Domain.Users.ValueObjects;

public sealed class Username : IEquatable<Username>
{
    private const int MinLength = 3;
    private const int MaxLength = 20;
    private const string Pattern = @"^[a-zA-Z0-9_.]+$";
    private static readonly Regex UsernamePattern = new(Pattern, RegexOptions.Compiled);

    public string Value { get; }

    public Username(string value)
    {
        Value = value;
    }

    public static ErrorOr<Username> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Username cannot be empty.");
        if (value.Length < MinLength)
            return Error.Validation(nameof(value), $"Username must be at least {MinLength} characters long.");
        if (value.Length > MaxLength)
            return Error.Validation(nameof(value), $"Username must be at most {MaxLength} characters long.");
        if (!UsernamePattern.IsMatch(value))
            return Error.Validation(nameof(value), "Username can only contain letters, numbers, underscores, and periods.");

        return new Username(value);
    }

    public static ErrorOr<Username> CreateByLogin(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Username cannot be empty.");
        if (value.Length < MinLength)
            return Error.Validation(nameof(value), "Username invalid.");
        if (value.Length > MaxLength)
            return Error.Validation(nameof(value), "Username invalid.");
        
        return new Username(value);
    }

    public override bool Equals(object? obj) => Equals(obj as Username);

    public bool Equals(Username? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}