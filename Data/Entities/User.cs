using System.ComponentModel.DataAnnotations;
using BackendLaboratory.Data.Entities.Enums;

namespace BackendLaboratory.Data.Entities
{
    public class User
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        [MaxLength(1000)]
        public required string FullName { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }

        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        public string? PhoneNumber { get; set; }

        public List<Community> Communities { get; set; } = new List<Community>();

        public List<CommunityUser> CommunityUsers { get; set; } = new List<CommunityUser>();
    }
}
