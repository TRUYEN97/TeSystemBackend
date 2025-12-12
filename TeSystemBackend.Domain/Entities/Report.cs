using TeSystemBackend.Domain.Enums;

namespace TeSystemBackend.Domain.Entities;

public class Report
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public ReportType Type { get; set; }
    public ReportStatus Status { get; set; }
    
    public int CreatedById { get; set; }
    
    public int? LocationId { get; set; }
    
    public DateTime ReportDate { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
    public int? UpdatedById { get; set; }
    public virtual AppUser CreatedBy { get; set; } = null!;
    public virtual AppUser? UpdatedBy { get; set; }
    public virtual Location? Location { get; set; }
}

