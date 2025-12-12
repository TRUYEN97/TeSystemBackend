using FluentValidation;
using TeSystemBackend.Application.DTOs.Reports;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Application.Validators.Reports;

public class UpdateReportDtoValidator : AbstractValidator<UpdateReportDto>
{
    public UpdateReportDtoValidator()
    {
        RuleFor(x => x.Title)
            .MaximumLength(500).WithMessage("Title must not exceed 500 characters")
            .When(x => !string.IsNullOrEmpty(x.Title));

        RuleFor(x => x.Status)
            .Must(status => !status.HasValue || Enum.IsDefined(typeof(ReportStatus), status.Value))
            .WithMessage("Invalid report status")
            .When(x => x.Status.HasValue);

        RuleFor(x => x.EndDate)
            .GreaterThan(x => x.StartDate)
            .When(x => x.StartDate.HasValue && x.EndDate.HasValue)
            .WithMessage("End date must be after start date");
    }
}

