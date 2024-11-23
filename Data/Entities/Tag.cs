using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class Tag
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string Name { get; set; }

        public List<Post> Posts { get; set; } = new List<Post>();
    }
}
