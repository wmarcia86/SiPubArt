using Domain.Articles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository class. Implements data access operations for Article entities.
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
public class ArticleRepository : IArticleRepository
{
    private readonly ApplicationDbContext _context;

    /// <summary>
    /// Constructor.
    /// Initializes a new instance of the ArticleRepository class.
    /// </summary>
    /// <param name="context">The application's database context.</param>
    public ArticleRepository(ApplicationDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }

    /// <summary>
    /// Adds a new article to the repository.
    /// </summary>
    /// <param name="article">The article to add.</param>
    public void Add(Article article) => 
        _context.Articles.Add(article);

    /// <summary>
    /// Retrieves an article by its identifier, including author and comments with their authors.
    /// </summary>
    /// <param name="id">The article identifier.</param>
    /// <returns>The article if found; otherwise, null.</returns>
    public async Task<Article?> GetByIdAsync(ArticleId id) => 
        await _context.Articles
            .Include(article => article.Author)
            .Include(article => article.Comments)
                .ThenInclude(comment => comment.Author)
            .SingleOrDefaultAsync(article => article.Id.Equals(id));

    /// <summary>
    /// Checks if an article exists by its identifier.
    /// </summary>
    /// <param name="id">The article identifier.</param>
    /// <returns>True if the article exists; otherwise, false.</returns>
    public async Task<bool> ExistsAsync(ArticleId id) => 
        await _context.Articles
            .AnyAsync(article => article.Id.Equals(id));

    /// <summary>
    /// Retrieves all articles, including their authors, ordered by publication date descending.
    /// </summary>
    /// <returns>A list of all articles.</returns>
    public async Task<List<Article>> GetAll() => 
        await _context.Articles
            .Include(article => article.Author)
            .Include(article => article.Comments)
                .ThenInclude(comment => comment.Author)
            .OrderByDescending(a => a.PublicationDate)
            .ToListAsync();

    /// <summary>
    /// Gets the total number of articles in the repository.
    /// </summary>
    /// <returns>The count of articles.</returns>
    public async Task<int> Count() => 
        await _context.Articles.CountAsync();

    /// <summary>
    /// Retrieves a paged list of articles, including their authors, ordered by publication date descending.
    /// </summary>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of articles for the specified page.</returns>
    public async Task<List<Article>> GetPaged(int pageNumber, int pageSize) =>
        await _context.Articles
            .Include(article => article.Author)
            .OrderByDescending(article => article.PublicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

    /// <summary>
    /// Updates an existing article in the repository.
    /// </summary>
    /// <param name="article">The article to update.</param>
    public void Update(Article article) => 
        _context.Articles.Update(article);

    /// <summary>
    /// Deletes an article from the repository.
    /// </summary>
    /// <param name="article">The article to delete.</param>
    public void Delete(Article article) => 
        _context.Articles.Remove(article);

    /// <summary>
    /// Gets the total number of articles for a specific author.
    /// </summary>
    /// <param name="authorID">The author's identifier.</param>
    /// <returns>The count of articles.</returns>
    public async Task<int> CountByAuthorAsync(UserId authorID) => 
        await _context.Articles
            .Where(article => article.AuthorId == authorID)
            .CountAsync();

    /// <summary>
    /// Retrieves a paged list of articles for a specific author, ordered by publication date descending.
    /// </summary>
    /// <param name="authorId">The author's identifier.</param>
    /// <param name="pageNumber">The page number.</param>
    /// <param name="pageSize">The page size.</param>
    /// <returns>A list of articles for the specified author and page.</returns>
    public async Task<List<Article>> GetByAuthorIdAsync(UserId authorId, int pageNumber, int pageSize) => 
        await _context.Articles
            .Where(article => article.AuthorId == authorId)
            .Include(article => article.Author)
            .OrderByDescending(article => article.PublicationDate)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
}
