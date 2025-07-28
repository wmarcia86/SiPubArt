using ErrorOr;
using MediatR;

namespace Application.Articles.Delete;

public record DeleteArticleCommand(Guid Id) : IRequest<ErrorOr<Guid>>;