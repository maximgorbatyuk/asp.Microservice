using asp.Microservice.Application.Features.Users;
using asp.Microservice.Application.Features.Users.CreateUser;
using asp.Microservice.Application.Features.Users.SearchUsers;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace asp.Microservice.Controllers;

[ApiController]
[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(
        IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Returns users.
    /// </summary>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Payment merchants.</returns>
    [HttpGet("")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<UserDto>))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IEnumerable<UserDto>> SearchAsync(
        CancellationToken cancellationToken)
    {
        return await _mediator.Send(new SearchUsersQuery(), cancellationToken);
    }

    /// <summary>
    /// Creates new user.
    /// </summary>
    /// <param name="request">Request.</param>
    /// <param name="cancellationToken">CancellationToken.</param>
    /// <returns>Payment contracts.</returns>
    [HttpPost("")]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(UserDto))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ProblemDetails))]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<UserDto> CreateAsync(
        [FromBody] CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        return await _mediator.Send(request, cancellationToken);
    }
}