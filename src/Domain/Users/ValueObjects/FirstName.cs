using ErrorOr;

namespace Domain.Users.ValueObjects;

/// <summary>
/// Value Object. Represents the first name of a user.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class FirstName
{
    private const int MinLength = 2;
    private const int MaxLength = 50;

    public string Value { get; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the FirstName class.
    /// </summary>
    /// <param name="value">The first name value.</param>
    public FirstName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new FirstName instance after validating the value.
    /// </summary>
    /// <param name="value">The first name value.</param>
    /// <returns>An ErrorOr containing the FirstName or an error.</returns>
    public static ErrorOr<FirstName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "First name cannot be empty.");
        if (value.Length < MinLength)
            return Error.Validation(nameof(value), $"First name must be at least {MinLength} characters long.");
        if (value.Length > MaxLength)
            return Error.Validation(nameof(value), $"First name must be at most {MaxLength} characters long.");

        return new FirstName(value);
    }

    public override bool Equals(object? obj) => Equals(obj as FirstName);

    public bool Equals(FirstName? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}