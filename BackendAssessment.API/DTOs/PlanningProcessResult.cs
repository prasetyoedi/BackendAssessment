using System.Collections.Generic;
using BackendAssessment.Persistence.Entities;

namespace BackendAssessment.API.DTOs;

public class PlanningProcessResult
{
    public bool IsSuccess { get; set; }
    public bool IsDuplicate { get; set; }
    public Planning? Data { get; set; }
    public List<ValidationError>? Errors { get; set; }

    public static PlanningProcessResult Success(Planning data) =>
        new() { IsSuccess = true, Data = data };

    public static PlanningProcessResult Duplicate(Planning data) =>
        new() { IsSuccess = true, IsDuplicate = true, Data = data };

    public static PlanningProcessResult Failure(List<ValidationError> errors) =>
        new() { IsSuccess = false, Errors = errors };
}