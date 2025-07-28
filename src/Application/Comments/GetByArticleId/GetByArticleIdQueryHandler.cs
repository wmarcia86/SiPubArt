using Application.Comments.Common;
using AutoMapper;
using Domain.Articles;
using Domain.Comments;
using ErrorOr;
using MediatR;

namespace Application.Comments.GetByArticleId;

internal sealed class GetByArticleIdQueryHandler : IRequestHandler<GetByArticleIdQuery, ErrorOr<IReadOnlyList<CommentResponse>>>
{
    private readonly ICommentRepository _commenteRepository;
    private readonly IMapper _mapper;

    public GetByArticleIdQueryHandler(ICommentRepository commentRepository, IMapper mapper)
    {
        _commenteRepository = commentRepository ?? throw new ArgumentNullException(nameof(commentRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<ErrorOr<IReadOnlyList<CommentResponse>>> Handle(GetByArticleIdQuery query, CancellationToken cancellationToken)
    {
        IReadOnlyList<Comment> comments = await _commenteRepository.GetByArticleIdAsync(new ArticleId(query.ArticleId));

        return comments.Select(comment => _mapper.Map<CommentResponse>(comment)).ToList(); ;
    }
}
