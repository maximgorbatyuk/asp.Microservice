using MediatR;

namespace asp.Microservice.Application.Features.Users.SearchUsers;

public class SearchUsersQuery : IRequest<IEnumerable<UserDto>>
{
}