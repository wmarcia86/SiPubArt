using ErrorOr;

namespace Domain.Articles.ValueObjects;

/// <summary>
/// Value Object. Represents the title of an article.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class ArticleTitle : IEquatable<ArticleTitle>
{
    private const int MinLength = 3;
    private const int MaxLength = 200;

    public string Value { get; set; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the ArticleTitle class.
    /// </summary>
    /// <param name="value">The title value.</param>
    public ArticleTitle(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new ArticleTitle instance after validating the value.
    /// </summary>
    /// <param name="value">The title value.</param>
    /// <returns>An ErrorOr containing the ArticleTitle or an error.</returns>
    public static ErrorOr<ArticleTitle> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Article title cannot be empty.", nameof(value));
        if (value.Length < MinLength)
            throw new ArgumentException($"Article title must be at least {MinLength} characters long.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"Article title must be at most {MaxLength} characters long.", nameof(value));

        return new ArticleTitle(value);
    }

    public override bool Equals(object? obj) => Equals(obj as ArticleTitle);

    public bool Equals(ArticleTitle? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}