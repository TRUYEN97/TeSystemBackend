using FluentValidation;
using TeSystemBackend.Application.DTOs.Auth;

namespace TeSystemBackend.Application.Validators.Auth;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
    public RegisterRequestValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Tên không được để trống")
            .MaximumLength(100).WithMessage("Tên không được dài quá 100 ký tự");

        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống")
            .MaximumLength(100).WithMessage("Username không được dài quá 100 ký tự");

        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email không được để trống")
            .EmailAddress().WithMessage("Email không hợp lệ");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống")
            .MinimumLength(6).WithMessage("Password phải có ít nhất 6 ký tự");
    }
}


