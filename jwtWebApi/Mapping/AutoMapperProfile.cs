using AutoMapper;
using jwtWebApi.Dto;
using jwtWebApi.Models;
using JwtWebApi.Dto;

public class AutoMapperProfile : Profile
{
    public AutoMapperProfile()
    {
        CreateMap<User, UserDtoResponse>();
        CreateMap<RefreshToken, RefreshTokenDto>();
    }
}