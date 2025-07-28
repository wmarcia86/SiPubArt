using Domain.Articles.ValueObjects;
using Domain.Comments;
using Domain.Core;
using Domain.Users;

namespace Domain.Articles;

/// <summary>
/// Entity. Represents an article published by a user.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class Article : AggregateRoot
{
    public ArticleId Id { get; set; }
    public ArticleTitle Title { get; set; }
    public ArticleContent Content { get; set; }
    public UserId AuthorId { get; set; }
    public DateTime PublicationDate { get; set; }
    public User? Author { get; set; }
    public ICollection<Comment> Comments { get; set; } = new List<Comment>();

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the Article class.
    /// </summary>
    /// <param name="id">The article identifier.</param>
    /// <param name="title">The article title.</param>
    /// <param name="content">The article content.</param>
    /// <param name="authorId">The author's identifier.</param>
    /// <param name="publicationDate">The publication date.</param>
    public Article(
        ArticleId id,
        ArticleTitle title,
        ArticleContent content,
        UserId authorId,
        DateTime publicationDate)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        Title = title ?? throw new ArgumentNullException(nameof(title));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        PublicationDate = publicationDate;
    }
}
