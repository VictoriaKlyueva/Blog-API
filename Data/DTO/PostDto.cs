using BackendLaboratory.Constants;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class PostDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Title { get; set; }

        [Required]
        [MinLength(1)]
        public required string Description { get; set; }

        [Required]
        public int ReadingTime { get; set; }

        public required string Image { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        [Required]
        [MinLength(1)]
        public required string Author { get; set; }

        public required string CommunityId { get; set; }
        public required string CommunityName { get; set; }
        public required string AddressId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.ValueMustBePositive)]
        public int Likes { get; set; } = 0;

        [Required]
        public bool HasLike { get; set; } = false;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.ValueMustBePositive)]
        public int CommentsCount { get; set; } = 0;

        public required List<Guid> Tags { get; set; }
    }
}
