using Application.Comments.Common;
using Application.Comments.Create;
using AutoMapper;
using Domain.Articles;
using Domain.Comments;
using Domain.Comments.ValueObjects;
using Domain.Users;

namespace Application.Comments.Mapper;

public class CommentProfile : Profile
{
    public CommentProfile()
    {
        CreateMap<Comment, CommentResponse>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Value))
            .ForCtorParam("authorId", opt => opt.MapFrom(src => src.AuthorId.Value))
            .ForCtorParam("author", opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : string.Empty))
            .ForCtorParam("publicationDate", opt => opt.MapFrom(src => src.PublicationDate.ToString("o")));

        CreateMap<CreateCommentCommand, Comment>()
            .ForCtorParam("id", opt => opt.MapFrom(_ => new CommentId(Guid.NewGuid())))
            .ForCtorParam("articleId", opt => opt.MapFrom(src => new ArticleId(src.ArticleId)))
            .ForCtorParam("content", opt => opt.MapFrom(src => new CommentContent(src.Content)))
            .ForCtorParam("authorId", opt => opt.MapFrom(src => new UserId(src.AuthorId)))
            .ForCtorParam("publicationDate", opt => opt.MapFrom(_ => DateTime.UtcNow));
    }
}
