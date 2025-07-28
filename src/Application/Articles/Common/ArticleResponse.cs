using Application.Comments.Common;

namespace Application.Articles.Common;

public record ArticleResponse(
    Guid Id,
    string Title,
    string Content,
    string AuthorId,
    string Author,
    string PublicationDate,
    List<CommentResponse> Comments
);
