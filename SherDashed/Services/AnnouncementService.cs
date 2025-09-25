using SherDashed.Models.Announcement;

namespace SherDashed.Services;

public class AnnouncementService
{
    private readonly JsonDataService _jsonService;
    private const string FileName = "announcements";

    public AnnouncementService(JsonDataService jsonService) => _jsonService = jsonService;

    private async Task<Announcement.AnnouncementRoot> LoadRoot() =>
        await _jsonService.LoadData<Announcement.AnnouncementRoot>(FileName);

    private async Task SaveRoot(Announcement.AnnouncementRoot root) => 
        await _jsonService.SaveData(FileName, root);

    public async Task<List<Announcement>> GetAll() => 
        (await LoadRoot()).Announcements;

    public async Task<Announcement?> GetById(int id) =>
        (await LoadRoot()).Announcements.FirstOrDefault(a => a.AnnouncementId == id);

    public async Task Add(Announcement announcement)
    {
        var root = await LoadRoot();

        if (announcement.AnnouncementId == 0)
        {
            announcement.AnnouncementId = root.Announcements.Any()
                ? root.Announcements.Max(a => a.AnnouncementId) + 1
                : 1;
        }

        if (announcement.CreatedOn == default)
        {
            announcement.CreatedOn = DateTime.Now;
        }

        announcement.ModifiedOn = null;
        
        root.Announcements.Add(announcement);
        
        await SaveRoot(root);
    }

    public async Task<bool> Update(Announcement announcement)
    {
        var root = await LoadRoot();
        var index = root.Announcements.FindIndex(a => a.AnnouncementId == announcement.AnnouncementId);

        if (index == -1) return false;

        announcement.ModifiedOn = DateTime.Now;
        root.Announcements[index] = announcement;
        await SaveRoot(root);
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var root = await LoadRoot();
        var removed = root.Announcements.FindIndex(a => a.AnnouncementId == id);

        if (removed == -1) return false;
        root.Announcements.RemoveAt(removed);
        await SaveRoot(root);
        return true;
    }
}