namespace TeSystemBackend.Application.DTOs.Reports;

public class CreateReportDto
{
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Type { get; set; } // ReportType enum value
    public int? LocationId { get; set; }
    public DateTime ReportDate { get; set; }
    public DateTime? StartDate { get; set; } // For custom reports
    public DateTime? EndDate { get; set; }   // For custom reports
}

