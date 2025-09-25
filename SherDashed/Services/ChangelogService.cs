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

}