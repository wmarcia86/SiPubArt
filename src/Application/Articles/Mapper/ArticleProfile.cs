using Application.Articles.Common;
using Application.Articles.Create;
using Application.Articles.Update;
using AutoMapper;
using Domain.Articles;
using Domain.Articles.ValueObjects;
using Domain.Users;

namespace Application.Articles.Mapper;

public class ArticleProfile : Profile
{
    public ArticleProfile()
    {
        CreateMap<Article, ArticleResponse>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title.Value))
            .ForMember(dest => dest.Content, opt => opt.MapFrom(src => src.Content.Value))
            .ForCtorParam("authorId", opt => opt.MapFrom(src => src.AuthorId.Value))
            .ForCtorParam("author", opt => opt.MapFrom(src => src.Author != null ? src.Author.FullName : string.Empty))
            .ForCtorParam("publicationDate", opt => opt.MapFrom(src => src.PublicationDate.ToString("o")))
            .ForCtorParam("comments", opt => opt.MapFrom(src => src.Comments));

        CreateMap<CreateArticleCommand, Article>()
            .ForCtorParam("id", opt => opt.MapFrom(_ => new ArticleId(Guid.NewGuid())))
            .ForCtorParam("title", opt => opt.MapFrom(src => new ArticleTitle(src.Title)))
            .ForCtorParam("content", opt => opt.MapFrom(src => new ArticleContent(src.Content)))
            .ForCtorParam("authorId", opt => opt.MapFrom(src => new UserId(src.AuthorId)))
            .ForCtorParam("publicationDate", opt => opt.MapFrom(_ => DateTime.UtcNow));

        CreateMap<UpdateArticleCommand, Article>()
            .ForCtorParam("title", opt => opt.MapFrom(src => new ArticleTitle(src.Title)))
            .ForCtorParam("content", opt => opt.MapFrom(src => new ArticleContent(src.Content)));
    }
}
