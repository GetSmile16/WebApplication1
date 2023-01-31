using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Film
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Image { get; set; }
        [Required]
        public string Description { get; set; }
        public string Type { get; set; }
        [Range(1, 10, ErrorMessage = "Недопустимый рейтинг")]
        public int Rating { get; set; }
    }
}
