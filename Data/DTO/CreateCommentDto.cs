using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class CreateCommentDto
    {
        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string Content { get; set; }

        public Guid? ParentId { get; set; }
    }
}
