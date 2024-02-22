using CourseProject.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CourseProject.Data;

public class CourseProjectDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, string>
{
    public CourseProjectDbContext(DbContextOptions<CourseProjectDbContext> options) : base(options)
    {
    }

    public DbSet<NewsPost> NewsPosts { get; set; }
    public DbSet<BoardGame> BoardGames { get; set; }
    public DbSet<ApplicationUser> Users { get; set; }
    public DbSet<ApplicationRole> Roles { get; set; }
}