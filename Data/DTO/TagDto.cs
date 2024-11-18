using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class TagDto
    {
        [Required]
        public Guid id { get; set; }

        [Required]
        public required DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string name { get; set; }
    }
}
