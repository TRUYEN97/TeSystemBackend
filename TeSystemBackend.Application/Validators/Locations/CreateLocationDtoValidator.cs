using FluentValidation;
using TeSystemBackend.Application.DTOs.Locations;

namespace TeSystemBackend.Application.Validators.Locations;

public class CreateLocationDtoValidator : AbstractValidator<CreateLocationDto>
{
    public CreateLocationDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Location name is required")
            .MaximumLength(200).WithMessage("Location name must not exceed 200 characters");
    }
}

