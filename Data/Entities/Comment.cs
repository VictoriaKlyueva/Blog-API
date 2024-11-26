using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class Comment
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Content { get; set; }

        public DateTime? ModifiedDate { get; set; }

        public DateTime? DeleteDate { get; set; }

        public Guid? ParentId { get; set; }

        public virtual Comment? ParentComment { get; set; }

        public virtual ICollection<Comment> ChildComments { get; set; } = new List<Comment>();

        [Required]
        public Guid AuthorId { get; set; }

        public Guid PostId { get; set; }
    }
}
