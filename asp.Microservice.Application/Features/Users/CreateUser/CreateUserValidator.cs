using FluentValidation;

namespace asp.Microservice.Application.Features.Users.CreateUser;

public class CreateUserValidator : AbstractValidator<CreateUserCommand>
{
    public CreateUserValidator()
    {
        RuleFor(x => x.Email)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.FirstName)
            .NotNull()
            .NotEmpty();

        RuleFor(x => x.LastName)
            .NotNull()
            .NotEmpty();
    }
}