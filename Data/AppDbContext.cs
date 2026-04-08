using CodeQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeQuest.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Chapter> Chapters => Set<Chapter>();
    public DbSet<Choice> Choices => Set<Choice>();
    public DbSet<GameSession> GameSessions => Set<GameSession>();
    public DbSet<AnswerRecord> AnswerRecords => Set<AnswerRecord>();
    public DbSet<LeaderboardEntry> LeaderboardEntries => Set<LeaderboardEntry>();

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<Chapter>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Choices)
             .WithOne(x => x.Chapter)
             .HasForeignKey(x => x.ChapterId)
             .OnDelete(DeleteBehavior.Cascade);
        });

        mb.Entity<GameSession>(e =>
        {
            e.HasKey(x => x.Id);
            e.HasMany(x => x.Answers)
             .WithOne(x => x.GameSession)
             .HasForeignKey(x => x.GameSessionId)
             .OnDelete(DeleteBehavior.Cascade);
            e.HasIndex(x => x.SessionKey);
        });

        mb.Entity<AnswerRecord>(e => e.HasKey(x => x.Id));
        mb.Entity<LeaderboardEntry>(e => e.HasKey(x => x.Id));
    }
}
