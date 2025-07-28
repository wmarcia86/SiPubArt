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

[ApiController]
[Route("api/articles")]
public class ArticlesController : ApiController
{
    private readonly ISender _mediator;

    public ArticlesController(ISender mediator)
    {
        _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
    }

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