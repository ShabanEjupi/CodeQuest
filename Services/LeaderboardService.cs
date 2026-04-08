using CodeQuest.Data;
using CodeQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeQuest.Services;

public interface ILeaderboardService
{
    Task RecordAsync(GameSession session);
    Task<List<LeaderboardEntry>> GetTopAsync(int count = 20);
}

public class LeaderboardService : ILeaderboardService
{
    private readonly AppDbContext _db;

    public LeaderboardService(AppDbContext db) => _db = db;

    public async Task RecordAsync(GameSession session)
    {
        // Only record completed sessions
        if (!session.IsComplete) return;

        // Avoid duplicates for same session
        bool exists = await _db.LeaderboardEntries
            .AnyAsync(e => e.PlayerName == session.PlayerName
                        && e.AchievedAt >= session.StartedAt.AddSeconds(-5));
        if (exists) return;

        _db.LeaderboardEntries.Add(new LeaderboardEntry
        {
            PlayerName     = session.PlayerName,
            Score          = session.Score,
            CorrectAnswers = session.CorrectAnswers,
            Rank           = session.Rank,
            AchievedAt     = session.CompletedAt ?? DateTime.UtcNow
        });
        await _db.SaveChangesAsync();
    }

    public Task<List<LeaderboardEntry>> GetTopAsync(int count = 20) =>
        _db.LeaderboardEntries
           .OrderByDescending(e => e.Score)
           .ThenByDescending(e => e.AchievedAt)
           .Take(count)
           .ToListAsync();
}
