using ErrorOr;
using MediatR;

namespace Application.Articles.Update;

public record UpdateArticleCommand(
    Guid Id,
    string Title,
    string Content
) : IRequest<ErrorOr<Guid>>;