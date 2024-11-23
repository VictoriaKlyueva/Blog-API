using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class CreatePostDto
    {
        [Required]
        [MinLength(5)]
        [MaxLength(1000)]
        public required string Title { get; set; }

        [Required]
        [MinLength(5)]
        [MaxLength(5000)]
        public required string Description { get; set; }

        [Required]
        public required int ReadingTime { get; set; }

        [MaxLength(1000)]
        public string? Image { get; set; }

        [Required]
        public List<Guid> Tags { get; set; } = new List<Guid>();
    }
}
