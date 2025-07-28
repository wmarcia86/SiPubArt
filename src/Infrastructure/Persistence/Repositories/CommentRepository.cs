using Domain.Articles;
using Domain.Comments;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository class. Implements data access operations for Comment entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class CommentRepository : ICommentRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the CommentRepository class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    public CommentRepository(ApplicationDbContext context)
    {   
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Adds a new comment to the repository.
    /// </summary>
    /// <param name="comment">The comment to add.</param>
    public void Add(Comment comment) =>
        _context.Comments.Add(comment);

    /// <summary>
    /// Retrieves a comment by its identifier, including the author.
    /// </summary>
    /// <param name="id">The comment identifier.</param>
    /// <returns>The comment if found; otherwise, null.</returns>
    public async Task<Comment?> GetByIdAsync(CommentId id) =>
        await _context.Comments
            .Include(comment => comment.Author)
            .SingleOrDefaultAsync(comment => comment.Id.Equals(id));

    /// <summary>
    /// Checks if a comment exists by its identifier.
    /// </summary>
    /// <param name="id">The comment identifier.</param>
    /// <returns>True if the comment exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(CommentId id) =>
        await _context.Articles
            .AnyAsync(comment => comment.Id.Equals(id));

    /// <summary>
    /// Retrieves all comments, including their authors, ordered by publication date descending.
    /// </summary>
    /// <returns>A list of all comments.</returns>
    public async Task<List<Comment>> GetAll() =>
        await _context.Comments
            .Include(comment => comment.Author)
            .OrderByDescending(comment => comment.PublicationDate)
            .ToListAsync();

    /// <summary>
    /// Gets the total number of comments in the repository.
    /// </summary>
    /// <returns>The count of comments.</returns>
    public async Task<int> Count() => await _context.Articles.CountAsync();

    /// <summary>
    /// Retrieves a paged list of comments, including their authors, ordered by publication date descending.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of comments for the specified page.</returns>
    public async Task<List<Comment>> GetPaged(int pageNumber, int pageSize) =>
        await _context.Comments
            .Include(comment => comment.Author)
            .OrderByDescending(comment => comment.PublicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    /// <summary>
    /// Updates an existing comment in the repository.
    /// </summary>
    /// <param name="comment">The comment to update.</param>
    public void Update(Comment comment) =>
        _context.Comments.Update(comment);

    /// <summary>
    /// Deletes a comment from the repository.
    /// </summary>
    /// <param name="comment">The comment to delete.</param>
    public void Delete(Comment comment) =>
        _context.Comments.Remove(comment);

    /// <summary>
    /// Retrieves a list of comments for a specific article, including their authors, ordered by publication date descending.
    /// </summary>
    /// <param name="articleId">The article identifier.</param>
    /// <returns>A list of comments for the specified article.</returns>
    public async Task<List<Comment>> GetByArticleIdAsync(ArticleId articleId) =>
        await _context.Comments
            .Where(comment => comment.ArticleId == articleId)
            .Include(comment => comment.Author)
            .OrderByDescending(comment => comment.PublicationDate)
            .ToListAsync();
}
