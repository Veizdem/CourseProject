using CourseProject.Repositories;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using CourseProject.Entities;
using CourseProject.Models;
using Microsoft.AspNetCore.Authorization;

namespace CourseProject.Controllers;

[Authorize(Roles = "менеджер")]
public class NewsPostController : Controller
{
    private readonly IRepository<NewsPost> _newsPostRepository;
    private readonly IWebHostEnvironment _environment;
    
    private readonly int pageSize = 9;

    public NewsPostController(IRepository<NewsPost> newsPostRepository, IWebHostEnvironment environment)
    {
        _newsPostRepository = newsPostRepository;
        _environment = environment;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Index(int page = 1)
    {
        var newsEntities = _newsPostRepository.GetAllAsync().Result;
        var totalNews = newsEntities.Count();
        
        var newsViewModels = newsEntities
            .OrderByDescending(n => n.PublishedOn)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(n => new NewsPostViewModel
            {
                Id = n.Id,
                Title = n.Title,
                ShortDescription = n.ShortDescription,
                Content = n.Content,
                ImagePath = n.ImagePath,
                PublishedOn = n.PublishedOn
            })
            .ToList();

        var viewModel = new NewsIndexViewModel
        {
            News = newsViewModels,
            CurrentPage = page,
            PageSize = pageSize,
            TotalNews = totalNews,
            TotalPages = (int)Math.Ceiling(totalNews/(double)pageSize)
        };

        return View(viewModel);
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var newsEntity = await _newsPostRepository.GetByIdAsync(id.Value);
        if (newsEntity == null)
        {
            return NotFound();
        }

        var newsViewModel = new NewsPostViewModel
        {
            Id = newsEntity.Id,
            Title = newsEntity.Title,
            ShortDescription = newsEntity.ShortDescription,
            Content = newsEntity.Content,
            ImagePath = newsEntity.ImagePath,
            PublishedOn = newsEntity.PublishedOn
        };

        return View(newsViewModel);
    }

    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(NewsPostViewModel model)
    {
        if (ModelState.IsValid)
        {
            string uniqueFileName = null;
            if (model.ImageFile != null && model.ImageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.ImageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await model.ImageFile.CopyToAsync(fileStream);
                }
            }
            else
            {
                uniqueFileName = "DefaultImage.png";
            }
            
            var newsEntity = new NewsPost
            {
                Title = model.Title,
                ShortDescription = model.ShortDescription,
                Content = model.Content,
                ImagePath = uniqueFileName,
                PublishedOn = DateTime.UtcNow
            };
            await _newsPostRepository.AddAsync(newsEntity);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var newsEntity = await _newsPostRepository.GetByIdAsync(id);
        if (newsEntity == null) return NotFound();
        
        var model = new NewsPostViewModel
        {
            Id = newsEntity.Id,
            Title = newsEntity.Title,
            ShortDescription = newsEntity.ShortDescription,
            Content = newsEntity.Content,
            ImagePath = newsEntity.ImagePath,
            PublishedOn = newsEntity.PublishedOn
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, NewsPostViewModel model, IFormFile imageFile)
    {
        if (id != model.Id) return NotFound();

        
        if (imageFile == null || imageFile.Length == 0)
        {
            ModelState.Remove("ImageFile");
        }
        
        if (ModelState.IsValid)
        {
            var newsEntity = await _newsPostRepository.GetByIdAsync(id);
            if (newsEntity == null) return NotFound();

            if (imageFile != null && imageFile.Length > 0)
            {
                string uploadsFolder = Path.Combine(_environment.WebRootPath, "images");
                string uniqueFileName = Guid.NewGuid().ToString() + "_" + imageFile.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await imageFile.CopyToAsync(fileStream);
                }
                newsEntity.ImagePath = uniqueFileName; 
            }

            newsEntity.Title = model.Title;
            newsEntity.Content = model.Content;
            newsEntity.ShortDescription = model.ShortDescription;
            newsEntity.PublishedOn = model.PublishedOn;

            await _newsPostRepository.UpdateAsync(newsEntity);
            return RedirectToAction(nameof(Index));
        }

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var newsEntity = await _newsPostRepository.GetByIdAsync(id);
        if (newsEntity == null) NotFound();

        await _newsPostRepository.DeleteAsync(id);
        return RedirectToAction(nameof(Index));
    }
}