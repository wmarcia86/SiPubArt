using ErrorOr;
using System.Text.RegularExpressions;

namespace Domain.Users.ValueObjects;

/// <summary>
/// Value Object. Represents the email of a user.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class Email : IEquatable<Email>
{
    private const int MinLength = 5;
    private const int MaxLength = 254;
    private const string Pattern = @"^[a-zA-Z0-9_!#$%&'\*+/=?{|}~^.-]+@[a-zA-Z0-9.-]+$";
    private static readonly Regex EmailPattern = new(Pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public string Value { get; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the Email class.
    /// </summary>
    /// <param name="value">The email value.</param>
    public Email(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new Email instance after validating the value.
    /// </summary>
    /// <param name="value">The email value.</param>
    /// <returns>An ErrorOr containing the Email or an error.</returns>
    public static ErrorOr<Email> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return Error.Validation(nameof(value), "Email cannot be empty.");
        if (value.Length < MinLength)
            return Error.Validation(nameof(value), $"Email must be at least {MinLength} characters long.");
        if (value.Length > MaxLength)
            return Error.Validation(nameof(value), $"Email must be at most {MaxLength} characters long.");
        if (!EmailPattern.IsMatch(value))
            return Error.Validation(nameof(value), "Email format is invalid.");

        return new Email(value);
    }

    public override bool Equals(object? obj) => Equals(obj as Email);

    public bool Equals(Email? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}