using SherDashed.Models.Changelog;

namespace SherDashed.Services;

public class ChangelogService
{
    private readonly JsonDataService _jsonDataService;
    private readonly string FileName = "changelog";

    public ChangelogService(JsonDataService jsonDataService)
    {
        _jsonDataService = jsonDataService;
    }

    public async Task InitializeAsync()
    {
        await _jsonDataService.InitializeFileAsync<Changelog>(FileName);
    }

    public async Task<List<Changelog>> GetAllAsync()
    {
        return await _jsonDataService.LoadListAsync<Changelog>(FileName);
    }

    public async Task<Changelog?> GetByIdAsync(int id)
    {
        return await _jsonDataService.FindInListAsync<Changelog>(FileName, c => c.ChangelogId == id);
    }

    public async Task AddAsync(Changelog changelog)
    {
        if (changelog.ChangelogId == 0)
        {
            var existing = await GetAllAsync();
            changelog.ChangelogId = existing.Count != 0 ? existing.Max(c => c.ChangelogId) + 1 : 1 ;
        }

        await _jsonDataService.AddToListAsync(FileName, changelog);
    }

    public async Task Update(Changelog changelog)
    {
        await _jsonDataService.UpdateInListAsync(FileName, c => c.ChangelogId == changelog.ChangelogId, changelog);
    }

    public async Task Delete(int id)
    {
        await _jsonDataService.RemoveFromListAsync<Changelog>(FileName, c => c.ChangelogId == id);
    }
}