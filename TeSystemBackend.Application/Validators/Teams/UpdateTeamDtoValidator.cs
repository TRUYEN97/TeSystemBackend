using FluentValidation;
using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Validators.Teams;

public class UpdateTeamDtoValidator : AbstractValidator<UpdateTeamDto>
{
    public UpdateTeamDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required")
            .MaximumLength(200).WithMessage("Team name must not exceed 200 characters");
    }
}

