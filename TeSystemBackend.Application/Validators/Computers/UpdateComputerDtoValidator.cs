using FluentValidation;
using TeSystemBackend.Application.DTOs.Computers;

namespace TeSystemBackend.Application.Validators.Computers;

public class UpdateComputerDtoValidator : AbstractValidator<UpdateComputerDto>
{
    public UpdateComputerDtoValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên máy tính không được để trống")
            .MaximumLength(200).WithMessage("Tên máy tính không được dài quá 200 ký tự");

        RuleFor(x => x.LocationId)
            .GreaterThan(0).WithMessage("Vị trí không hợp lệ");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Mô tả không được dài quá 1000 ký tự");
    }
}

