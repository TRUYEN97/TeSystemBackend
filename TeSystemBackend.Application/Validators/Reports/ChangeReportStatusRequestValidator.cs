using FluentValidation;
using TeSystemBackend.Application.DTOs.Reports;
using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Application.Validators.Reports;

public class ChangeReportStatusRequestValidator : AbstractValidator<ChangeReportStatusRequest>
{
    public ChangeReportStatusRequestValidator()
    {
        RuleFor(x => x.Status)
            .Must(status => Enum.IsDefined(typeof(ReportStatus), status))
            .WithMessage("Invalid report status");
    }
}

