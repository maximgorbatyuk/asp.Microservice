using asp.Microservice.Application.Features.Users;
using asp.Microservice.Domain.Entities;
using AutoMapper;

namespace asp.Microservice.Infrastructure.MapperProfiles;

public class UserMapperProfile : Profile
{
    public UserMapperProfile()
    {
        CreateMap<User, UserDto>();
    }
}