using CourseProject.Data;
using CourseProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Repositories;

public class NewsPostRepository : IRepository<NewsPost>
{
    private readonly CourseProjectDbContext _context;

    public NewsPostRepository(CourseProjectDbContext context)
    {
        _context = context;
    }
    
    public async Task<NewsPost> GetByIdAsync(int id)
    {
        return await _context.NewsPosts.FindAsync(id);
    }

    public async Task<IEnumerable<NewsPost>> GetAllAsync()
    {
        return await _context.NewsPosts.ToListAsync();
    }

    public async Task AddAsync(NewsPost entity)
    {
        await _context.NewsPosts.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(NewsPost entity)
    {
        _context.NewsPosts.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.NewsPosts.FindAsync(id);
        _context.NewsPosts.Remove(entity);
        await _context.SaveChangesAsync();
    }
}