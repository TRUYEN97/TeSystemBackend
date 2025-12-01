using FluentValidation;
using TeSystemBackend.Application.DTOs.Computers;

namespace TeSystemBackend.Application.Validators.Computers;

public class UpdateComputerDtoValidator : AbstractValidator<UpdateComputerDto>
{
    public UpdateComputerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Computer name is required")
            .MaximumLength(200).WithMessage("Computer name must not exceed 200 characters");

        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("Location is invalid");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description must not exceed 1000 characters");
    }
}



