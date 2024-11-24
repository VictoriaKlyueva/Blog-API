using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class CommentDto
    {
        [Required]
        public required string Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Content { get; set; }

        public DateTime? ModifiedDate { get; set; }
        public DateTime? DeleteDate { get; set; }

        [Required]
        public required string AuthorId { get; set; }

        [Required]
        [MinLength(1)]
        public required string Author { get; set; }

        public int SubComments { get; set; }
    }
}
