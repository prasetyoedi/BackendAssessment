using System.Collections.Generic;

namespace BackendAssessment.API.DTOs;

public class PlanningRequestDto
{
    public string RequestCode { get; set; } = string.Empty;
    public List<int> Slots { get; set; } = new List<int>();
    public string? CandidateToken { get; set; }
}