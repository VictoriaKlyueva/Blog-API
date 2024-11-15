using BackendLaboratory.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class UserDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public required DateTime CreateTime { get; set; }

        [Required]
        [MinLength(1)]
        public required string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender Gender { get; set; }

        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

        public string? PhoneNumber { get; set; }
    }
}
