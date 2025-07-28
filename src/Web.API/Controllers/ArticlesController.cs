using Application.Articles.Create;
using Application.Articles.Delete;
using Application.Articles.GetAll;
using Application.Articles.GetByAuthorId;
using Application.Articles.GetById;
using Application.Articles.Update;
using Application.Comments.Create;
using Application.Comments.GetByArticleId;
using ErrorOr;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Web.API.Controllers.Base;

namespace Web.API.Controllers;

/// <summary>
/// Controller class for managing article-related API endpoints.
/// Type: Controller
/// Author: WMarcia
/// Date: 2025-07-28
/// </summary>
[ApiController]
[Route("api/articles")]
public class ArticlesController : ApiController
{
    private readonly ISender _mediator;

    /// <summary>
    /// Initializes a new instance of the <see cref="ArticlesController"/> class.
    /// </summary>
    /// <param name="mediator">The mediator instance for handling requests.</param>
    public ArticlesController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

    /// <summary>
    /// Creates a new article.
    /// </summary>
    /// <param name="command">The command containing article creation data.</param>
    /// <returns>The result of the creation operation.</returns>
    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create([FromBody] CreateArticleCommand command)
    {
        var createResult = await _mediator.Send(command);

        return createResult.Match(
            articleId => Ok(articleId),
            errors => Problem(errors)
        );
    }
    
    /// <summary>
    /// Retrieves an article by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the article.</param>
    /// <returns>The requested article or an error.</returns>
    [HttpGet("{id}")]
    [Authorize]
    public async Task<IActionResult> GetById(Guid id)
    {
        var getByIdResult = await _mediator.Send(new GetArticleByIdQuery(id));

        return getByIdResult.Match(
            article => Ok(article),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves a paged list of articles.
    /// </summary>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of articles per page.</param>
    /// <returns>A paged list of articles.</returns>
    [HttpGet]
    [Authorize]
    [EnableQuery]
    public async Task<IActionResult> GetPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var getPagedResult = await _mediator.Send(new GetPagedArticlesQuery(pageNumber, pageSize));

        return getPagedResult.Match(
            pagedArticles => Ok(pagedArticles),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves articles by author identifier with paging.
    /// </summary>
    /// <param name="authorId">The unique identifier of the author.</param>
    /// <param name="pageNumber">The page number to retrieve.</param>
    /// <param name="pageSize">The number of articles per page.</param>
    /// <returns>A paged list of articles by the specified author.</returns>
    [HttpGet("author/{authorId}")]
    [Authorize]
    [EnableQuery]
    public async Task<IActionResult> GetByAuthorId(Guid authorId, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
    {
        var role = "admin"; // User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value;

        if (role == "admin")
        {
            var getPagedResult = await _mediator.Send(new GetPagedArticlesQuery(pageNumber, pageSize));

            return getPagedResult.Match(
                pagedArticles => Ok(pagedArticles),
                errors => Problem(errors)
            );
        }
        else
        {
            var getByAuthorIdResult = await _mediator.Send(new GetArticlesByAuthorIdQuery(authorId, pageNumber, pageSize));

            return getByAuthorIdResult.Match(
                articles => Ok(articles),
                errors => Problem(errors)
            );
        }
    }

    /// <summary>
    /// Updates an existing article.
    /// </summary>
    /// <param name="id">The unique identifier of the article to update.</param>
    /// <param name="command">The command containing updated article data.</param>
    /// <returns>The result of the update operation.</returns>
    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateArticleCommand command)
    {
        if (command.Id != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("Article.UpdateInvalid", "The request Id does not match with the url Id.")
            };

            return Problem(errors);
        }

        var updateResult = await _mediator.Send(command);

        return updateResult.Match(
            articleId => Ok(articleId),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Deletes an article by its unique identifier.
    /// </summary>
    /// <param name="id">The unique identifier of the article to delete.</param>
    /// <returns>No content if successful, or an error.</returns>
    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(Guid id)
    {
        var deleteResult = await _mediator.Send(new DeleteArticleCommand(id));

        return deleteResult.Match(
            articleId => NoContent(),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Retrieves comments for a specific article.
    /// </summary>
    /// <param name="articleId">The unique identifier of the article.</param>
    /// <returns>A list of comments for the specified article.</returns>
    [HttpGet("{id}/comments")]
    [Authorize]
    [EnableQuery]
    public async Task<IActionResult> GetByArticleId(Guid articleId)
    {
        var getByArticleIdResult = await _mediator.Send(new GetByArticleIdQuery(articleId));

        return getByArticleIdResult.Match(
            comments => Ok(comments),
            errors => Problem(errors)
        );
    }

    /// <summary>
    /// Creates a new comment for a specific article.
    /// </summary>
    /// <param name="id">The unique identifier of the article.</param>
    /// <param name="command">The command containing comment creation data.</param>
    /// <returns>The result of the comment creation operation.</returns>
    [HttpPost("{id}/comments")]
    [Authorize]
    public async Task<IActionResult> CreateComment(Guid id, [FromBody] CreateCommentCommand command)
    {
        if (command.ArticleId != id)
        {
            List<Error> errors = new()
            {
                Error.Validation("Article.Comment.CreateInvalid", "The request.ArticleId Id does not match with the url Id.")
            };

            return Problem(errors);
        }

        var createCommentResult = await _mediator.Send(command);

        return createCommentResult.Match(
            commentId => Ok(commentId),
            errors => Problem(errors)
        );
    }
}