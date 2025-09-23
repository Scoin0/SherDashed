using System.ComponentModel.DataAnnotations;

namespace SherDashed.Models.Changelog;

public class Changelog
{
    public int ChangelogId { get; set; }
    [Required]
    public string Version { get; set; } = "0.0.0";
    [Required]
    public List<ChangelogEntry> EntryDescription { get; set; } = [];
    [Required]
    public DateTime EntryDate { get; set; }
}