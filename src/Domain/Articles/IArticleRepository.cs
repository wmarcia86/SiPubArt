using Domain.Common;
using Domain.Users;

namespace Domain.Articles;

/// <summary>
/// Repository interface. Defines data access operations for Article entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public interface IArticleRepository : IRepository<Article, ArticleId>
{
    /// <summary>
    /// Gets the total number of articles for a specific author.
    /// </summary>
    /// <param name="authorID">The author's identifier.</param>
    /// <returns>The count of articles.</returns>
    Task<int> CountByAuthorAsync(UserId authorID);

    /// <summary>
    /// Retrieves a paged list of articles for a specific author.
    /// </summary>
    /// <param name="authorId">The author's identifier.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of articles.</returns>
    Task<List<Article>> GetByAuthorIdAsync(UserId authorId, int pageNumber, int pageSize);
}