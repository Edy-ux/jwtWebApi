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
        CreateMap<User, UserDtoResponse>();
        // .ForMember(dest => dest.RefreshTokens, opt => opt.MapFrom(src => src.RefreshTokens.Where(rf => !rf.Revoked).ToList()));
        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}
