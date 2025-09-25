using Microsoft.AspNetCore.Mvc;

namespace SherDashed.Controllers;

[Route("[controller]")]
public class ChangelogController : Controller
{

    // GET: /changelog
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        return View();
    }
    
    // GET: /changelog/details/{id}
    [HttpGet("details/{id:int}")]
    public async Task<IActionResult> Details()
    {
        return View();
    }
    
    // GET: /changelog/create
    // POST: /chaneglog/create
    // GET: /changelog/edit/{id}
    // POST: /changelog/edit/{id}
    // POST: /changelog/delete/{id}
    
}