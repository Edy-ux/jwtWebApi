using AutoMapper;
using jwtWebApi.Dto;
using jwtWebApi.Dto.User;
using jwtWebApi.Models;
using JwtWebApi.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>()
            .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore());
        CreateMap<User, UserDtoResponse>();
        //    .ForMember(dest => dest.RefreshTokens, opt => opt.Ignore());
        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}
