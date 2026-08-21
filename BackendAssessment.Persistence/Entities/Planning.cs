using System;
using System.Collections.Generic;

namespace BackendAssessment.Persistence.Entities;

public class Planning
{
    public int Id { get; set; }
    public string RequestCode { get; set; } = string.Empty;
    public string CandidateToken { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "PROCESSED";
    public int Version { get; set; } = 1;

    public ICollection<PlanningSlot> Slots { get; set; } = new List<PlanningSlot>();
}