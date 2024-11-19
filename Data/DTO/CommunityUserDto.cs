using BackendLaboratory.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class CommunityUserDto
    {
        [Required]
        public Guid UserId { get; set; }

        [Required]
        public Guid CommunityId { get; set; }

        [Required]
        public CommunityRole Role { get; set; }
    }
}
