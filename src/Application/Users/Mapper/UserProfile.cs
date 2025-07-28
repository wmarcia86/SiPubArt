using Application.Users.Common;
using Application.Users.Create;
using Application.Users.Update;
using AutoMapper;
using Domain.Users;
using Domain.Users.ValueObjects;

namespace Application.Users.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserResponse>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id.Value))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName.Value))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName.Value))
            .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Username.Value))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.Value))
            .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
            .ForMember(dest => dest.Active, opt => opt.MapFrom(src => src.Active));

        CreateMap<CreateUserCommand, User>()
            .ForCtorParam("id", opt => opt.MapFrom(_ => new UserId(Guid.NewGuid())))
            .ForCtorParam("firstName", opt => opt.MapFrom(src => new FirstName(src.FirstName)))
            .ForCtorParam("lastName", opt => opt.MapFrom(src => new LastName(src.LastName)))
            .ForCtorParam("username", opt => opt.MapFrom(src => new Username(src.Username)))
            .ForCtorParam("email", opt => opt.MapFrom(src => new Email(src.Email)))
            .ForCtorParam("password", opt => opt.MapFrom(src => new Password(src.Password)))
            .ForCtorParam("role", opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role, true)))
            .ForCtorParam("active", opt => opt.MapFrom(_ => true));

        CreateMap<UpdateUserCommand, User>()
            .ForCtorParam("id", opt => opt.MapFrom(src => src.Id))
            .ForCtorParam("firstName", opt => opt.MapFrom(src => new FirstName(src.FirstName)))
            .ForCtorParam("lastName", opt => opt.MapFrom(src => new LastName(src.LastName)))
            .ForCtorParam("username", opt => opt.MapFrom(src => new Username(src.Username)))
            .ForCtorParam("email", opt => opt.MapFrom(src => new Email(src.Email)))
            .ForCtorParam("password", opt => opt.MapFrom(src => new Password(src.Password)))
            .ForCtorParam("role", opt => opt.MapFrom(src => Enum.Parse<UserRole>(src.Role, true)))
            .ForCtorParam("active", opt => opt.MapFrom(src => src.Active));
    }
}
