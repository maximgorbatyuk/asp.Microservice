using MediatR;

namespace asp.Microservice.Application.Features.Users.CreateUser;

public record CreateUserCommand : IRequest<UserDto>
{
    public string? Email { get; init; }

    public string? FirstName { get; init; }

    public string? LastName { get; init; }
}