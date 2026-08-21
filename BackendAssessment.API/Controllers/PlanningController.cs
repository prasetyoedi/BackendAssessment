using Microsoft.AspNetCore.Mvc;
using BackendAssessment.API.DTOs;
using BackendAssessment.API.Services;
using BackendAssessment.Persistence.DbContext;
using Microsoft.EntityFrameworkCore;
using BackendAssessment.Persistence.Entities;

namespace BackendAssessment.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlanningController : ControllerBase
{
    private readonly IPlanningService _planningService;
    private readonly AppDbContext _context;

    public PlanningController(IPlanningService planningService, AppDbContext context)
    {
        _planningService = planningService;
        _context = context;
    }

    // POST: api/planning
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] PlanningRequestDto request)
    {
        var result = await _planningService.CreatePlanningAsync(request);
        if (!result.IsSuccess && result.Errors != null)
        {
            return BadRequest(new
            {
                message = "Validasi gagal.",
                errors = result.Errors.Select(e => new { e.Field, e.Message })
            });
        }

        if (result.IsDuplicate)
        {
            return Ok(new
            {
                message = "RequestCode sudah diproses sebelumnya.",
                planning = MapToResponseDto(result.Data!)
            });
        }

        var responseDto = MapToResponseDto(result.Data!);
        return CreatedAtAction(nameof(GetDetail), new { id = responseDto.Id }, responseDto);
    }

    // GET: api/planning/history
    [HttpGet("history")]
    public async Task<IActionResult> GetHistory([FromQuery] int page = 1, [FromQuery] int limit = 10)
    {
        if (page < 1) page = 1;
        if (limit < 1) limit = 10;
        if (limit > 100) limit = 100;

        var data = await _context.Plannings
            .Include(p => p.Slots)
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * limit)
            .Take(limit)
            .Select(p => new
            {
                p.Id,
                p.RequestCode,
                p.CreatedAt,
                p.Status,
                OriginalTotal = p.Slots.Sum(s => s.OriginalQuantity),
                BalancedTotal = p.Slots.Sum(s => s.BalancedQuantity),
                ActiveSlots = p.Slots.Count(s => s.IsActive)
            })
            .ToListAsync();

        return Ok(data);
    }

    // GET: api/planning/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetDetail(int id)
    {
        var planning = await _context.Plannings
            .Include(p => p.Slots)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (planning == null)
            return NotFound(new { message = $"Planning dengan ID {id} tidak ditemukan." });

        return Ok(MapToResponseDto(planning));
    }

    private PlanningResponseDto MapToResponseDto(Planning planning)
    {
        return new PlanningResponseDto
        {
            Id = planning.Id,
            RequestCode = planning.RequestCode,
            CandidateToken = planning.CandidateToken,
            CreatedAt = planning.CreatedAt,
            Status = planning.Status,
            Version = planning.Version,
            Slots = planning.Slots.Select(s => new PlanningSlotDto
            {
                Id = s.Id,
                SlotOrder = s.SlotOrder,
                SlotName = s.SlotName,
                OriginalQuantity = s.OriginalQuantity,
                BalancedQuantity = s.BalancedQuantity,
                IsActive = s.IsActive
            }).OrderBy(s => s.SlotOrder).ToList()
        };
    }
}