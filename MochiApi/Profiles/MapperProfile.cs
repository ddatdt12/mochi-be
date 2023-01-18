using AutoMapper;
using MochiApi.Dtos;
using MochiApi.Models;

public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<int?, int>().ConvertUsing((src, dest) => src ?? dest);

        CreateMap<User, UserDto>();
        CreateMap<UserDto, User>();
        CreateMap<RegisterUserDto, User>();
        CreateMap<LoginUserDto, User>();
    }
}