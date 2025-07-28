using ErrorOr;

namespace Domain.Articles.ValueObjects;

/// <summary>
/// Value Object. Represents the content of an article.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class ArticleContent : IEquatable<ArticleContent>
{
    private const int MinLength = 10;
    private const int MaxLength = 10000;

    public string Value { get; set; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the ArticleContent class.
    /// </summary>
    /// <param name="value">The content value.</param>
    public ArticleContent(string value)
    {
        Value = value;
    }

    /// <summary>
    /// Creates a new ArticleContent instance after validating the value.
    /// </summary>
    /// <param name="value">The content value.</param>
    /// <returns>An ErrorOr containing the ArticleContent or an error.</returns>
    public static ErrorOr<ArticleContent> Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Article content cannot be empty.", nameof(value));
        if (value.Length < MinLength)
            throw new ArgumentException($"Article content must be at least {MinLength} characters long.", nameof(value));
        if (value.Length > MaxLength)
            throw new ArgumentException($"Article content must be at most {MaxLength} characters long.", nameof(value));

        return new ArticleContent(value);
    }

    public override bool Equals(object? obj) => Equals(obj as ArticleContent);

    public bool Equals(ArticleContent? other) => other is not null && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();

    public override string ToString() => Value;
}