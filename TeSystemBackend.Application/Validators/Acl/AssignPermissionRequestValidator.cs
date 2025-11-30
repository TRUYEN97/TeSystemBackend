using FluentValidation;
using TeSystemBackend.Application.DTOs.Acl;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Application.Validators.Acl;

public class AssignPermissionRequestValidator : AbstractValidator<AssignPermissionRequest>
{
    public AssignPermissionRequestValidator()
    {
        RuleFor(x => x.ResourceType)
            .NotEmpty().WithMessage("Loại resource không được để trống");

        RuleFor(x => x.ResourceId)
            .GreaterThan(0).WithMessage("Resource ID không hợp lệ");

        RuleFor(x => x.SubjectType)
            .Must(t => Enum.IsDefined(typeof(PrincipalType), t))
            .WithMessage("Subject type không hợp lệ. Phải là: 0 (User), 1 (Team), 2 (TeamRoleLocation), 3 (Role)");

        RuleFor(x => x.SubjectId)
            .GreaterThan(0).WithMessage("Subject ID không hợp lệ");

        RuleFor(x => x.PermissionId)
            .GreaterThan(0).WithMessage("Permission ID không hợp lệ");
    }
}

