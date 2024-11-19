using BackendLaboratory.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class CommunityUser
    {
        [Required]
        public Guid UserId { get; set; }

        public required User User { get; set; }

        [Required]
        public Guid CommunityId { get; set; }

        public required Community Community { get; set; }

        [Required]
        public CommunityRole Role { get; set; }
    }
}
