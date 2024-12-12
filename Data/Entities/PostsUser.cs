using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class PostsUser
    {
        [Required]
        public Guid UserId { get; set; }

        public User User { get; set; }

        [Required]
        public Guid PostId { get; set; }

        public Post Post { get; set; }

        public bool EmailStatus { get; set; }
    }
}
