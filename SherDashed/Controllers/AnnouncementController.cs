using Microsoft.AspNetCore.Mvc;
using SherDashed.Services;

namespace SherDashed.Controllers;

public class AnnouncementController : Controller
{
    private readonly AnnouncementService _announcementService;

    public AnnouncementController(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    public async Task<IActionResult> Index()
    {
        var announcements = await _announcementService.GetAllAsync();
        return View(announcements);
    }
}