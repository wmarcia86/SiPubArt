using Domain.Articles;
using Domain.Common;

namespace Domain.Comments;

/// <summary>
/// Repository interface. Defines data access operations for Comment entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public interface ICommentRepository : IRepository<Comment, CommentId>
{
    /// <summary>
    /// Retrieves a list of comments for a specific article.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>A list of comments.</returns>
    Task<List<Comment>> GetByArticleIdAsync(ArticleId articleId);
}