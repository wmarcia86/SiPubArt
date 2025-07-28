namespace Application.Comments.Common;

public record CommentResponse(
    Guid Id,
    string Content,
    string AuthorId,
    string Author,
    string PublicationDate
);
