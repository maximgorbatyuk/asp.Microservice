using asp.Microservice.Application.Database;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace asp.Microservice.Application.Features.Users.SearchUsers;

public class SearchUsersHandler : IRequestHandler<SearchUsersQuery, IEnumerable<UserDto>>
{
    private readonly IDatabaseContext _context;
    private readonly IMapper _mapper;

    public SearchUsersHandler(
        IDatabaseContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<UserDto>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        var users = await _context.Users.ToListAsync(cancellationToken);
        return _mapper.Map<IEnumerable<UserDto>>(users);
    }
}