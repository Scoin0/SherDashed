using Microsoft.AspNetCore.Mvc;
using SherDashed.Models.Announcement;
using SherDashed.Services;

namespace SherDashed.Controllers;

[Route("[controller]")]
public class AnnouncementController : Controller
{
    private readonly AnnouncementService _announcementService;

    public AnnouncementController(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }
    
    // GET: /announcement
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var announcements = await _announcementService.GetAll();
        return View(announcements);
    }

    // GET: /announcement/details/{id}
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details(int id)
    {
        var announcement = await _announcementService.GetById(id);
        return announcement == null ? NotFound() : View(announcement);
    }
    
    // GET: /announcement/create
    [HttpGet("create")]
    public IActionResult Create()
    {
        var announcement = new Announcement
        {
            CreatedOn = DateTime.Now
        };
        return View(announcement);
    }

    // POST: /announcement/create
    [HttpPost("create")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([FromForm] Announcement announcement)
    {
        if (ModelState.IsValid)
        {
            await _announcementService.Add(announcement);
            return RedirectToAction(nameof(Index));
        }
        return View(announcement);
    }

    // GET: /announcement/edit/{id}
    [HttpGet("edit/{id:int}")]
    public async Task<IActionResult> Edit(int id)
    {
        var announcement = await _announcementService.GetById(id);
        return announcement == null ? NotFound() : View(announcement);
    }
    
    // POST: /announcement/edit/{id}
    [HttpPost("edit/{id:int}")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [FromForm] Announcement announcement)
    {
        if (id != announcement.AnnouncementId)
            return BadRequest();

        if (ModelState.IsValid)
        {
            try
            {
                await _announcementService.Update(announcement);
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException)
            {
                return NotFound();
            }
        }
        return View(announcement);
    }
    
    // POST: /announcement/delete/{id}
    [HttpPost("delete/{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _announcementService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
        catch (InvalidOperationException)
        {
            return NotFound();
        }
    }
}