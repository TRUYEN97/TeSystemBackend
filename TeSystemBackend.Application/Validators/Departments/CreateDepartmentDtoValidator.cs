using FluentValidation;
using TeSystemBackend.Application.DTOs.Departments;

namespace TeSystemBackend.Application.Validators.Departments;

public class CreateDepartmentDtoValidator : AbstractValidator<CreateDepartmentDto>
{
    public CreateDepartmentDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Department name must not exceed 100 characters");
    }
}

