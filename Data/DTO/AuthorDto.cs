using BackendLaboratory.Data.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.DTO
{
    public class AuthorDto
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        [MinLength(1)]
        public required string FullName { get; set; }

        public DateTime? BirthDate { get; set; }

        [Required]
        public Gender gender { get; set; }

        public int? Posts { get; set; }

        public int? Likes { get; set; }

        public DateTime? Created { get; set; }
    }
}