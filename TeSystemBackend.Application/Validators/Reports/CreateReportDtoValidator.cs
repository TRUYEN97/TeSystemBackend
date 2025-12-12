using FluentValidation;
using TeSystemBackend.Application.DTOs.Reports;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Application.Validators.Reports;

public class CreateReportDtoValidator : AbstractValidator<CreateReportDto>
{
    public CreateReportDtoValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(500).WithMessage("Title must not exceed 500 characters");

        RuleFor(x => x.Content)
            .NotEmpty().WithMessage("Content is required");

        RuleFor(x => x.Type)
            .Must(type => Enum.IsDefined(typeof(ReportType), type))
            .WithMessage("Invalid report type");

        RuleFor(x => x.ReportDate)
            .NotEmpty().WithMessage("Report date is required");

        When(x => x.Type == (int)ReportType.Custom, () =>
        {
            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start date is required for custom reports");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End date is required for custom reports")
                .GreaterThan(x => x.StartDate)
                .When(x => x.StartDate.HasValue)
                .WithMessage("End date must be after start date");
        });

        When(x => x.Type == (int)ReportType.Daily || x.Type == (int)ReportType.Weekly, () =>
        {
            RuleFor(x => x.StartDate)
                .Null().WithMessage("Start date should not be provided for daily or weekly reports");

            RuleFor(x => x.EndDate)
                .Null().WithMessage("End date should not be provided for daily or weekly reports");
        });
    }
}

