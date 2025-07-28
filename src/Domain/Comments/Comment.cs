using Domain.Articles;
using Domain.Comments.ValueObjects;
using Domain.Core;
using Domain.Users;

namespace Domain.Comments;

/// <summary>
/// Entity. Represents a comment made by a user on an article.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class Comment : AggregateRoot
{
    public CommentId Id { get; set; }
    public ArticleId ArticleId { get; set; }
    public CommentContent Content { get; set; }
    public UserId AuthorId { get; set; }
    public DateTime PublicationDate { get; set; }
    public User? Author { get; set; }

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the Comment class.
    /// </summary>
    /// <param name="id">The comment identifier.</param>
    /// <param name="articleId">The article identifier.</param>
    /// <param name="content">The comment content.</param>
    /// <param name="authorId">The author's identifier.</param>
    /// <param name="publicationDate">The publication date.</param>
    public Comment(
        CommentId id,
        ArticleId articleId,
        CommentContent content,
        UserId authorId,
        DateTime publicationDate)
    {
        Id = id ?? throw new ArgumentNullException(nameof(id));
        ArticleId = articleId ?? throw new ArgumentNullException(nameof(articleId));
        Content = content ?? throw new ArgumentNullException(nameof(content));
        AuthorId = authorId ?? throw new ArgumentNullException(nameof(authorId));
        PublicationDate = publicationDate;
    }
}
