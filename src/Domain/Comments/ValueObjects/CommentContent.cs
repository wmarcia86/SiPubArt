using ErrorOr;

namespace Domain.Comments.ValueObjects;

/// <summary>
/// Value Object. Represents the content of a comment.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class CommentContent : IEquatable<CommentContent>
{
    private const int MinLength = 10;
    private const int MaxLength = 1000;

    public string Value { get; set; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the CommentContent class.
    /// </summary>
    /// <param name="value">The comment content value.</param>
    public CommentContent(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new CommentContent instance after validating the value.
    /// </summary>
    /// <param name="value">The comment content value.</param>
    /// <returns>An ErrorOr containing the CommentContent or an error.</returns>
    public static ErrorOr<CommentContent> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Article content cannot be empty.", nameof(value));
        if (value.Length < MinLength)
            throw new ArgumentException($"Article content must be at least {MinLength} characters long.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"Article content must be at most {MaxLength} characters long.", nameof(value));

        return new CommentContent(value);
    }

    public override bool Equals(object? obj) => Equals(obj as CommentContent);

    public bool Equals(CommentContent? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}
