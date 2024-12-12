using BackendLaboratory.Constants;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class Post
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
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.ValueMustBePositive)]
        public int ReadingTime { get; set; }

        public string? Image { get; set; }

        [Required]
        public Guid AuthorId { get; set; }

        public Guid? CommunityId { get; set; }

        public Guid? AddressId { get; set; }

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.ValueMustBePositive)]
        public int Likes { get; set; } = 0;

        [Required]
        [Range(0, int.MaxValue, ErrorMessage = ErrorMessages.ValueMustBePositive)]
        public int CommentsCount { get; set; } = 0;

        public List<Tag> Tags { get; set; } = new List<Tag>();

        public List<PostTag> PostTags {  get; set; } = new List<PostTag>();

        public List<User> Users { get; set; } = new List<User>();

        public List<Like> LikesLink { get; set; } = new List<Like>();

        public List<PostsUser> PostsUsers { get; set; } = new List<PostsUser>();
    }
}