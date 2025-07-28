using AutoMapper;
using Domain.Articles;
using Domain.Comments;
using Domain.Comments.ValueObjects;
using Domain.Core;
using ErrorOr;
using MediatR;

namespace Application.Comments.Create;

internal sealed class CreateCommentCommandHandler : IRequestHandler<CreateCommentCommand, ErrorOr<Guid>>
{
    private readonly ICommentRepository _commentRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CreateCommentCommandHandler(ICommentRepository commentRepository, IUnitOfWork unitOfWork, IMapper mapper)
    {
        _commentRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<Guid>> Handle(CreateCommentCommand command, CancellationToken cancellationToken)
    {
        var contentResult = CommentContent.Create(command.Content);

        if (command.ArticleId == Guid.Empty)
        {
            return Error.Validation(nameof(command.ArticleId), "Please provide a valid article id.");
        }

        if (contentResult.Value is not CommentContent)
        {
            return contentResult.Errors;
        }

        if (command.AuthorId == Guid.Empty)
        {
            return Error.Validation(nameof(command.AuthorId), "Please provide a valid author id.");
        }

        var comment = _mapper.Map<Comment>(command);

        _commentRepository.Add(comment);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return comment.Id.Value;
    }
}