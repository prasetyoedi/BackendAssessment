using System;
using System.Collections.Generic;

namespace BackendAssessment.API.DTOs;

public class PlanningResponseDto
{
    public int Id { get; set; }
    public string RequestCode { get; set; } = string.Empty;
    public string CandidateToken { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string Status { get; set; } = string.Empty;
    public int Version { get; set; }
    public List<PlanningSlotDto> Slots { get; set; } = new();
}

public class PlanningSlotDto
{
    public int Id { get; set; }
    public int SlotOrder { get; set; }
    public string SlotName { get; set; } = string.Empty;
    public int OriginalQuantity { get; set; }
    public int BalancedQuantity { get; set; }
    public bool IsActive { get; set; }
}