using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BackendAssessment.Domain.Services;
public static class BalancingService
{
    public static List<int> Balance(List<int> original)
    {
        if (original == null)
            throw new ArgumentNullException(nameof(original), "Input tidak boleh null.");

        if (original.Any(x => x < 0))
            throw new ArgumentException("Nilai negatif tidak diperbolehkan.", nameof(original));

        int n = original.Count;
        List<int> result = new List<int>(original);

        var activeIndices = original
            .Select((val, idx) => new { val, idx })
            .Where(x => x.val > 0)
            .ToList();

        int activeCount = activeIndices.Count;
        if (activeCount == 0) return result;

        int total = original.Sum();
        int baseValue = total / activeCount;
        int remainder = total % activeCount;

        foreach (var item in activeIndices)
            result[item.idx] = baseValue;

        var prioritySlots = activeIndices
            .OrderByDescending(x => x.val)
            .ThenBy(x => x.idx)
            .Take(remainder)
            .ToList();

        foreach (var item in prioritySlots)
            result[item.idx] += 1;

        return result;
    }
}