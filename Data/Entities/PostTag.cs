using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class PostTag
    {
        [Required]
        public Guid PostId { get; set; }

        [Required]
        public Post Post { get; set; }

        [Required]
        public Guid TagId { get; set; }

        [Required]
        public Tag Tag { get; set; }
    }
}
