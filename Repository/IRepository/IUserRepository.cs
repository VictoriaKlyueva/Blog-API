using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;

namespace BackendLaboratory.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string email);
        Task<TokenResponse> Login(LoginCredentials loginCredentials);
        Task<TokenResponse> Register(UserRegisterModel userRegisterModel);
        Task Logout(string userId);
        Task<UserDto> GetProfile(string token);
        Task EditProfile(string token, UserEditModel userEditModel);
    }
}
