using System.Threading.Tasks;
using BackendAssessment.API.DTOs;

namespace BackendAssessment.API.Services;

public interface IPlanningService
{
    Task<PlanningProcessResult> CreatePlanningAsync(PlanningRequestDto request);
}