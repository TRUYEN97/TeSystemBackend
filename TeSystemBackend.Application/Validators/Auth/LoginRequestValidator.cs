using FluentValidation;
using TeSystemBackend.Application.DTOs.Auth;

namespace TeSystemBackend.Application.Validators.Auth;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty().WithMessage("Username không được để trống");

        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password không được để trống");
    }
}


