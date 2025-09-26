using SherDashed.Models.Changelog;

namespace SherDashed.Services;

public class ChangelogService
{
    private readonly JsonDataService _jsonDataService;
    private const string FileName = "changelog";

    public ChangelogService(JsonDataService jsonDataService) => _jsonDataService = jsonDataService;

    private async Task<Changelog.ChangelogRoot> LoadRoot() =>
        await _jsonDataService.LoadData<Changelog.ChangelogRoot>(FileName);

    private async Task SaveRoot(Changelog.ChangelogRoot root) => 
        await _jsonDataService.SaveData(FileName, root);

    public async Task<List<Changelog>> GetAll() => 
        (await LoadRoot()).Changelogs;

    public async Task<Changelog?> GetById(int id) =>
        (await LoadRoot()).Changelogs.FirstOrDefault(c => c.ChangelogId == id);
    
    public async Task<List<Changelog>> GetRecent(int count)
    {
        var root = await LoadRoot();
        return root.Changelogs.OrderByDescending(c => c.EntryDate).Take(count).ToList();
    }
    
    public async Task Add(Changelog changelog)
    {
        var root = await LoadRoot();

        if (changelog.ChangelogId == 0)
        {
            changelog.ChangelogId = root.Changelogs.Any()
                ? root.Changelogs.Max(c => c.ChangelogId) + 1
                : 1;
        }

        if (changelog.EntryDate == default)
        {
            changelog.EntryDate = DateTime.Now;
        }

        root.Changelogs.Add(changelog);
        await SaveRoot(root);
    }

    public async Task<bool> Update(Changelog changelog)
    {
        var root = await LoadRoot();
        var index = root.Changelogs.FindIndex(c => c.ChangelogId == changelog.ChangelogId);

        if (index == -1) return false;
        root.Changelogs[index] = changelog;
        await SaveRoot(root);
        return true;
    }

    public async Task<bool> Delete(int id)
    {
        var root = await LoadRoot();
        var removed = root.Changelogs.FindIndex(c => c.ChangelogId == id);

        if (removed == -1) return false;
        root.Changelogs.RemoveAt(removed);
        await SaveRoot(root);
        return true;
    }
}