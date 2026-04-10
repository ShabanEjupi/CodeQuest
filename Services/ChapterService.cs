using CodeQuest.Data;
using CodeQuest.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

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
    private readonly IMemoryCache _cache;

    public ChapterService(AppDbContext db, IMemoryCache cache)
    {
        _db = db;
        _cache = cache;
    }

    public async Task<List<Chapter>> GetAllAsync(string language)
    {
        var cacheKey = $"Chapters_All_{language}";
        if (!_cache.TryGetValue(cacheKey, out List<Chapter>? chapters))
        {
            chapters = await _db.Chapters
               .AsNoTracking()
               .Where(c => c.Language == language)
               .Include(c => c.Choices.OrderBy(ch => ch.OrderIndex))
               .OrderBy(c => c.OrderIndex)
               .ToListAsync();
            _cache.Set(cacheKey, chapters, TimeSpan.FromHours(1));
        }
        return chapters!;
    }

    public async Task<Chapter?> GetByIndexAsync(int index, string language)
    {
        var all = await GetAllAsync(language);
        return index >= 0 && index < all.Count ? all[index] : null;
    }

    public async Task<int> GetTotalCountAsync(string language)
    {
        var all = await GetAllAsync(language);
        return all.Count;
    }
}
