using BackendLaboratory.Data.Entities.Enums;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class UserRegisterModel
    {
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
    }
}
