using System.ComponentModel.DataAnnotations;

namespace BackendLaboratory.Data.Entities
{
    public class TokenResponse
    {
        [Required]
        public required string Token { get; set; }
    }
}
