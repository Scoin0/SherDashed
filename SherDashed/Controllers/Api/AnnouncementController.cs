using Microsoft.AspNetCore.Mvc;
using SherDashed.Models.Announcement;
using SherDashed.Services;

namespace SherDashed.Controllers.Api;

[ApiController]
[Route("api/v1/[controller]")]
public class AnnouncementController : ControllerBase
{
    private readonly AnnouncementService _announcementService;

    public AnnouncementController(AnnouncementService announcementService)
    {
        _announcementService = announcementService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Announcement>>> GetAllAnnouncements()
    {
        var announcements = await _announcementService.GetAll();
        return Ok(announcements);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Announcement>> GetAnnouncementById(int id)
    {
        var announcement = await _announcementService.GetById(id);
        return announcement == null ? NotFound($"The announcement with the ID {id} was not found.") : Ok(announcement);
    }
}