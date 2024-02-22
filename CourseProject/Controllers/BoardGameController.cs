using CourseProject.Entities;
using CourseProject.Models;
using CourseProject.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CourseProject.Controllers;

[Authorize(Roles = "менеджер")]
public class BoardGameController : Controller
{
    private readonly IRepository<BoardGame> _boardGameRepository;
    private readonly IWebHostEnvironment _webHostEnvironment;
    private readonly int pageSize = 12;

    public BoardGameController(IRepository<BoardGame> boardGameRepository, IWebHostEnvironment webHostEnvironment)
    {
        _boardGameRepository = boardGameRepository;
        _webHostEnvironment = webHostEnvironment;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1)
    {
        var boardGames = await _boardGameRepository.GetAllAsync();
        var totalItems = boardGames.Count();
        var totalPages = (int)Math.Ceiling(totalItems / (double)pageSize);

        var viewModel = new BoardGamesIndexViewModel
        {
            BoardGames = boardGames
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(b => new BoardGameViewModel
                {
                    Id = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    ImagePaths = b.ImagePaths,
                    Price = b.Price
                })
                .ToList(),

            CurrentPage = page,
            PageSize = pageSize,
            TotalGames = totalItems,
            TotalPages = totalPages
        };

        return View(viewModel);
    }

    [HttpGet]
    public IActionResult Create() => View();

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(BoardGameViewModel model)
    {
        if (ModelState.IsValid)
        {
            var boardGame = new BoardGame
            {
                Name = model.Name,
                Description = model.Description,
                Price = model.Price,
                ImagePaths = new List<string>()
            };

            if (model.ImageFiles != null && model.ImageFiles.Count > 0)
            {
                foreach (var file in model.ImageFiles)
                {
                    var fileName = $"{Guid.NewGuid()}_{file.FileName}";
                    var filePath = Path.Combine(_webHostEnvironment.WebRootPath, "images/gameImages", fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }

                    boardGame.ImagePaths.Add(fileName);
                }
            }
            else
            {
                boardGame.ImagePaths.Add("DefaultGameImage.png");
            }

            await _boardGameRepository.AddAsync(boardGame);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int id)
    {
        var boardGame = await _boardGameRepository.GetByIdAsync(id);
        if (boardGame == null) return NotFound();

        var viewModel = new BoardGameViewModel
        {
            Id = boardGame.Id,
            Name = boardGame.Name,
            Description = boardGame.Description,
            Price = boardGame.Price,
            ImagePaths = boardGame.ImagePaths
        };

        return View(viewModel);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var boardGame = await _boardGameRepository.GetByIdAsync(id);
        if (boardGame == null) return NotFound();

        var viewModel = new BoardGameViewModel
        {
            Id = boardGame.Id,
            Name = boardGame.Name,
            Description = boardGame.Description,
            Price = boardGame.Price,
            ImagePaths = boardGame.ImagePaths
        };

        return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, BoardGameViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var boardGame = await _boardGameRepository.GetByIdAsync(id);
        if (boardGame == null) return NotFound();

        boardGame.Name = model.Name;
        boardGame.Description = model.Description;
        boardGame.Price = model.Price;

        await _boardGameRepository.UpdateAsync(boardGame);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var boardGame = await _boardGameRepository.GetByIdAsync(id);
        if (boardGame == null) return NotFound();

        await _boardGameRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}