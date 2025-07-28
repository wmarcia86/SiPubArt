using ErrorOr;

namespace Domain.Users.ValueObjects;

/// <summary>
/// Value Object. Represents the last name of a user.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class LastName
{
    private const int MinLength = 2;
    private const int MaxLength = 50;

    public string Value { get; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the LastName class.
    /// </summary>
    /// <param name="value">The last name value.</param>
    public LastName(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new LastName instance after validating the value.
    /// </summary>
    /// <param name="value">The last name value.</param>
    /// <returns>An ErrorOr containing the LastName or an error.</returns>
    public static ErrorOr<LastName> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Last name cannot be empty.");
        if (value.Length < MinLength)
            return Error.Validation(nameof(value), $"Last name must be at least {MinLength} characters long.");
        if (value.Length > MaxLength)
            return Error.Validation(nameof(value), $"Last name must be at most {MaxLength} characters long.");

        return new LastName(value);
    }

    public override bool Equals(object? obj) => Equals(obj as LastName);

    public bool Equals(LastName? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}