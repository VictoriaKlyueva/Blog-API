using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using Microsoft.AspNetCore.Identity.Data;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);
        Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO);
        Task Logout(string userId);
    }
}
