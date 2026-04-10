using CodeQuest.Data;
using CodeQuest.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeQuest.Services;

public interface IChapterService
{
    Task<List<Chapter>> GetAllAsync(string language);
    Task<Chapter?> GetByIndexAsync(int index, string language);
    Task<int> GetTotalCountAsync(string language);
}

public class ChapterService : IChapterService
{
    private readonly AppDbContext _db;

    public ChapterService(AppDbContext db) => _db = db;

    public Task<List<Chapter>> GetAllAsync(string language) =>
        _db.Chapters
           .AsNoTracking()
           .Where(c => c.Language == language)
           .Include(c => c.Choices.OrderBy(ch => ch.OrderIndex))
           .OrderBy(c => c.OrderIndex)
           .ToListAsync();

    public Task<Chapter?> GetByIndexAsync(int index, string language) =>
        _db.Chapters
           .AsNoTracking()
           .Where(c => c.Language == language)
           .Include(c => c.Choices.OrderBy(ch => ch.OrderIndex))
           .OrderBy(c => c.OrderIndex)
           .Skip(index)
           .FirstOrDefaultAsync();

    public Task<int> GetTotalCountAsync(string language) => _db.Chapters.Where(c => c.Language == language).CountAsync();
}
