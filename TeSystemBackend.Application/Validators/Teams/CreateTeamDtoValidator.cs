using FluentValidation;
using TeSystemBackend.Application.DTOs.Teams;

namespace TeSystemBackend.Application.Validators.Teams;

public class CreateTeamDtoValidator : AbstractValidator<CreateTeamDto>
{
    public CreateTeamDtoValidator()
    {
        RuleFor(x => x.DepartmentId)
            .GreaterThan(0).WithMessage("Department is required");

        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Team name is required")
            .MaximumLength(200).WithMessage("Team name must not exceed 200 characters");

        RuleFor(x => x.FullName)
            .NotEmpty().WithMessage("Team full name is required")
            .MaximumLength(400).WithMessage("Team full name must not exceed 400 characters");
    }
}

