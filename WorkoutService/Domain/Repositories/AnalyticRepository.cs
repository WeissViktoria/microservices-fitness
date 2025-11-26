using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace Domain.Repositories;

public class AnalyticRepository: ARepositoryAsync<History>
{
    public AnalyticRepository(DbContext context) : base(context)
    {
        
    }

    /// <summary>
    /// Get analysis for a specific participant within a time period
    /// </summary>
    public async Task<History?> GetAnalysisByParticipantAndPeriodAsync(int participantId, DateTime periodStart, DateTime periodEnd)
    {
        return await _context.Set<History>()
            .Where(h => h.ParticipantId == participantId 
                     && h.PeriodStart <= periodStart 
                     && h.PeriodEnd >= periodEnd)
            .OrderByDescending(h => h.CreatedAt)
            .FirstOrDefaultAsync();
    }

    /// <summary>
    /// Get all historical analyses for a participant
    /// </summary>
    public async Task<List<History>> GetHistoricalAnalysesAsync(int participantId, DateTime? periodStart = null, DateTime? periodEnd = null)
    {
        var query = _context.Set<History>()
            .Where(h => h.ParticipantId == participantId);

        if (periodStart.HasValue)
        {
            query = query.Where(h => h.PeriodStart >= periodStart.Value);
        }

        if (periodEnd.HasValue)
        {
            query = query.Where(h => h.PeriodEnd <= periodEnd.Value);
        }

        return await query
            .OrderBy(h => h.PeriodStart)
            .ToListAsync();
    }

    /// <summary>
    /// Get the latest analysis for a participant
    /// </summary>
    public async Task<History?> GetLatestAnalysisAsync(int participantId)
    {
        return await _context.Set<History>()
            .Where(h => h.ParticipantId == participantId)
            .OrderByDescending(h => h.CreatedAt)
            .FirstOrDefaultAsync();
    }
}