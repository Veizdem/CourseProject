namespace CourseProject.Models;
using System.ComponentModel.DataAnnotations;

public class NewsPostViewModel
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Поле 'Заголовок' є обов'язковим.")]
    [StringLength(100, ErrorMessage = "Довжина 'Заголовка' не повинна перевищувати 100 символів.")]
    public string Title { get; set; }
    
    public IFormFile? ImageFile { get; set; }
    
    [Required(ErrorMessage = "Поле 'Короткий опис' є обов'язковим.")]
    [StringLength(255, ErrorMessage = "Довжина 'Короткого опису' не повинна перевищувати 255 символів.")]
    public string ShortDescription { get; set; }
    
    [Required(ErrorMessage = "Поле 'Текст' є обов'язковим.")]
    public string Content { get; set; }
    
    public DateTime PublishedOn { get; set; }

    public string? ImagePath { get; set; }
}