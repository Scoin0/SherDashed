using SherDashed.Models.Announcement;

namespace SherDashed.Services;

public class AnnouncementService
{
    private readonly JsonDataService _jsonService;
    private readonly string FileName = "announcements";

    public AnnouncementService(JsonDataService jsonService)
    {
        _jsonService = jsonService;
    }

    public async Task InitializeAsync()
    {
        await _jsonService.InitializeFileAsync<Announcement>(FileName);
    }
    
    public async Task<List<Announcement>> GetAllAsync()
    {
        return await _jsonService.LoadListAsync<Announcement>(FileName);
    }

    public async Task<Announcement?> GetByIdAsync(int id)
    {
        return await _jsonService.FindInListAsync<Announcement>(FileName, a => a.AnnouncementId == id);
    }

    public async Task AddAsync(Announcement announcement)
    {
        if (announcement.AnnouncementId == 0)
        {
            var existing = await GetAllAsync();
            announcement.AnnouncementId = existing.Count != 0 ? existing.Max(a => a.AnnouncementId) + 1 : 1;
        }

        await _jsonService.AddToListAsync(FileName, announcement);
    }

    public async Task UpdateAsync(Announcement announcement)
    {
        await _jsonService.UpdateInListAsync(FileName, a => a.AnnouncementId == announcement.AnnouncementId, announcement);
    }

    public async Task DeleteAsync(int id)
    {
        await _jsonService.RemoveFromListAsync<Announcement>(FileName, a => a.AnnouncementId == id);
    }
}