using Domain.Common;

namespace Domain.Articles;

/// <summary>
/// Typed Id. Represents the unique identifier for an article.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public sealed class ArticleId : StronglyTypedId<ArticleId>
{
    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the ArticleId class.
    /// </summary>
    /// <param name="value">The GUID value for the article identifier.</param>
    public ArticleId(Guid value) : base(value) { }
}
