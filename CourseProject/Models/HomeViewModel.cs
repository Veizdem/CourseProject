using CourseProject.Entities;

namespace CourseProject.Models;

public class HomeViewModel
{
    public List<BoardGame> RandomBoardGames { get; set; }
    public List<NewsPost> LatestNewsPosts { get; set; }
}