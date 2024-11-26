using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class UpdateCommentDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Content { get; set; }
    }
}
