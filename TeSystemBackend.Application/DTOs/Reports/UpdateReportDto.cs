namespace TeSystemBackend.Application.DTOs.Reports;

public class UpdateReportDto
{
    public string? Title { get; set; }
    public string? Content { get; set; }
    public int? Status { get; set; } // ReportStatus enum value
    public int? LocationId { get; set; }
    public DateTime? ReportDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}

