using asp.Microservice.Application.Database;
using asp.Microservice.Domain.Entities;
using asp.Microservice.Domain.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace asp.Microservice.Application.Features.Users.CreateUser;

public class CreateUserHandler : IRequestHandler<CreateUserCommand, UserDto>
{
    private readonly IDatabaseContext _context;
    private readonly IMapper _mapper;

    public CreateUserHandler(
        IDatabaseContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(
        CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        if (await _context.Users.AnyAsync(x => x.Email == request.Email, cancellationToken))
        {
            throw new BadRequestException("User with this email already exists");
        }

        var user = new User(
            request.Email!,
            request.FirstName!,
            request.LastName!);

        await _context.Users.AddAsync(user, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return _mapper.Map<UserDto>(user);
    }
}