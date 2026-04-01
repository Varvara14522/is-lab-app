using System.ComponentModel.DataAnnotations;

namespace IsLabApp.Models;

public class Note
{
    public int Id { get; set; }
    
    [Required(ErrorMessage = "Заголовок обязателен")]
    [StringLength(200, MinimumLength = 3, ErrorMessage = "Заголовок от 3 до 200 символов")]
    public string Title { get; set; } = string.Empty;
    
    [StringLength(2000, ErrorMessage = "Текст не более 2000 символов")]
    public string? Text { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
