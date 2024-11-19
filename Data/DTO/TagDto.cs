using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class TagDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Name { get; set; }
    }
}
