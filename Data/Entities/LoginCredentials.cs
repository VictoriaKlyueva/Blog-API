using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class LoginCredentials
    {
        [Required]
        [MinLength(1)]
        public required string Email { get; set; }

        [Required]
        [MinLength(6)]
        public required string Password { get; set; }
    }
}
