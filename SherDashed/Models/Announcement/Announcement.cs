using System.ComponentModel.DataAnnotations;

namespace SherDashed.Models.Announcement;

public class Announcement
{
    public int AnnouncementId { get; set; }
    [Required]
    public string AnnouncementMessage { get; set; } = string.Empty;
}