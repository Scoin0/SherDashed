namespace SherDashed.Models.Changelog;

public class ChangelogEntry
{
    public int ChangelogEntryId { get; set; }
    public string Description { get; set; } = string.Empty;
    public ChangelogType ChangeType { get; set; } = ChangelogType.None;
}