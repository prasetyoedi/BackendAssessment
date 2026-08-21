using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BackendAssessment.API.DTOs;
using BackendAssessment.Domain.Services;
using BackendAssessment.Persistence.DbContext;
using BackendAssessment.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace BackendAssessment.API.Services;

public class PlanningService : IPlanningService
{
    private readonly AppDbContext _context;
    private readonly ILogger<PlanningService> _logger;

    public PlanningService(AppDbContext context, ILogger<PlanningService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<PlanningProcessResult> CreatePlanningAsync(PlanningRequestDto request)
    {
        var errors = new List<ValidationError>();

        if (string.IsNullOrWhiteSpace(request.RequestCode))
            errors.Add(new ValidationError("RequestCode", "RequestCode wajib diisi."));

        if (request.Slots == null || request.Slots.Count == 0)
        {
            errors.Add(new ValidationError("Slots", "Minimal harus ada 1 slot."));
        }
        else
        {
            for (int i = 0; i < request.Slots.Count; i++)
            {
                if (request.Slots[i] < 0)
                    errors.Add(new ValidationError($"Slots[{i}]", $"Slot ke-{i + 1} tidak boleh negatif."));
            }

            long totalLong = 0;
            foreach (var val in request.Slots)
            {
                totalLong += val;
                if (totalLong > int.MaxValue)
                {
                    errors.Add(new ValidationError("Slots", $"Total produksi ({totalLong}) melebihi batas maksimum integer (2.147.483.647)."));
                    break;
                }
            }
        }

        if (errors.Any())
            return PlanningProcessResult.Failure(errors);

        var existing = await _context.Plannings
            .Include(p => p.Slots)
            .FirstOrDefaultAsync(p => p.RequestCode == request.RequestCode);

        if (existing != null)
        {
            _logger.LogInformation("RequestCode duplikat: {RequestCode}", request.RequestCode);
            return PlanningProcessResult.Duplicate(existing);
        }

        List<int> balancedResult;
        try
        {
            balancedResult = BalancingService.Balance(request.Slots);
        }
        catch (OverflowException ex)
        {
            _logger.LogError(ex, "Overflow saat balancing.");
            return PlanningProcessResult.Failure(new List<ValidationError>
            {
                new ValidationError("Slots", ex.Message)
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error balancing.");
            return PlanningProcessResult.Failure(new List<ValidationError>
            {
                new ValidationError("Internal", "Terjadi kesalahan internal saat memproses balancing.")
            });
        }

        string[] dayNames = { "Senin", "Selasa", "Rabu", "Kamis", "Jumat", "Sabtu", "Minggu" };
        var planning = new Planning
        {
            RequestCode = request.RequestCode,
            CandidateToken = request.CandidateToken ?? "VEH-EDI_BACKEND",
            CreatedAt = DateTime.UtcNow,
            Status = "PROCESSED",
            Version = 1
        };

        for (int i = 0; i < request.Slots.Count; i++)
        {
            string slotName = (i < dayNames.Length) ? dayNames[i] : $"Slot-{i + 1}";

            planning.Slots.Add(new PlanningSlot
            {
                SlotOrder = i,
                SlotName = slotName,
                OriginalQuantity = request.Slots[i],
                BalancedQuantity = balancedResult[i],
                IsActive = request.Slots[i] > 0
            });
        }

        await using var transaction = await _context.Database.BeginTransactionAsync();
        try
        {
            _context.Plannings.Add(planning);
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            _logger.LogInformation("Planning berhasil disimpan: {RequestCode}", request.RequestCode);
            return PlanningProcessResult.Success(planning);
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "Gagal menyimpan Planning: {RequestCode}", request.RequestCode);

            return PlanningProcessResult.Failure(new List<ValidationError>
            {
                new ValidationError("Database", "Gagal menyimpan data. Tidak ada data parsial yang tersimpan.")
            });
        }
    }
}