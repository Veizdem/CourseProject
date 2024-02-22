using System.Diagnostics;
using CourseProject.Data;
using Microsoft.AspNetCore.Mvc;
using CourseProject.Models;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly CourseProjectDbContext _context;

    public HomeController(ILogger<HomeController> logger, CourseProjectDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task<IActionResult> Index()
    {
        var randomGames = await _context.BoardGames
            .OrderBy(r => Guid.NewGuid())
            .Take(3)
            .ToListAsync();

        var latestNews = await _context.NewsPosts
            .OrderByDescending(n => n.PublishedOn)
            .Take(3)
            .ToListAsync();

        var viewModel = new HomeViewModel
        {
            RandomBoardGames = randomGames,
            LatestNewsPosts = latestNews
        };

        return View(viewModel);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}