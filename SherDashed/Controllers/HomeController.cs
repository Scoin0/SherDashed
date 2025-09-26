using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SherDashed.Models;
using SherDashed.Models.Announcement;
using SherDashed.Services;

namespace SherDashed.Controllers;

public class HomeController : Controller
{
    private readonly AnnouncementService _announcementService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger, AnnouncementService announcementService)
    {
        _announcementService = announcementService;
        _logger = logger;
    }
    
    public async Task<IActionResult> Index()
    {
        var announcements = await _announcementService.GetAll();
        var latest = announcements.OrderByDescending(c => c.AnnouncementMessage).FirstOrDefault();
        var vm = new HomeModel()
        {
            LatestAnnouncement = latest
        };
        return View(vm);
    }
    
    public IActionResult Privacy()
    {
        return View();
    }
    
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}