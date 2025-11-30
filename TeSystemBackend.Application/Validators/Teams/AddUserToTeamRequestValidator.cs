using FluentValidation;
using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Validators.Teams;

public class AddUserToTeamRequestValidator : AbstractValidator<AddUserToTeamRequest>
{
    public AddUserToTeamRequestValidator()
    {
        RuleFor(x => x.UserId)
            .GreaterThan(0).WithMessage("User ID không hợp lệ");
    }
}

