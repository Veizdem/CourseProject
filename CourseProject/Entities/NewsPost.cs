namespace CourseProject.Entities;

public class NewsPost
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? ImagePath { get; set; }
    public string? ShortDescription { get; set; }
    public string? Content { get; set; }
    public DateTime PublishedOn { get; set; }
}