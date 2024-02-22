using System.ComponentModel.DataAnnotations;

namespace CourseProject.Models;

public class BoardGameViewModel
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Поле 'Назва' є обов'язковим.")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Поле 'Опис' є обов'язковим.")]
    public string Description { get; set; }

    [Required(ErrorMessage = "Поле 'Ціна' є обов'язковим.")]
    [Range(0, double.MaxValue, ErrorMessage = "Ціна повинна бути більше нуля.")]
    public decimal Price { get; set; }

    public List<IFormFile> ImageFiles { get; set; } = new List<IFormFile>();

    public List<string> ImagePaths { get; set; } = new List<string>();
}