using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Data.DTO
{
    public class LoginResponseDTO
    {
        public LocalUser User { get; set; }
        public string Token { get; set; }
    }
}
