using CourseProject.Data;
using CourseProject.Entities;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Repositories;

public class BoardGameRepository : IRepository<BoardGame>
{
    private readonly CourseProjectDbContext _context;

    public BoardGameRepository(CourseProjectDbContext context)
    {
        _context = context;
    }
    public async Task<BoardGame> GetByIdAsync(int id)
    {
        return await _context.BoardGames.FindAsync(id);
    }

    public async Task<IEnumerable<BoardGame>> GetAllAsync()
    {
        return await _context.BoardGames.ToListAsync();
    }

    public async Task AddAsync(BoardGame entity)
    {
        await _context.BoardGames.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(BoardGame entity)
    {
        _context.BoardGames.Update(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var entity = await _context.BoardGames.FindAsync(id);
        _context.BoardGames.Remove(entity);
        await _context.SaveChangesAsync();
    }
}