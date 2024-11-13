using BackendLaboratory.Data;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;

namespace BackendLaboratory.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _db;
        private string secretKey;

        public UserRepository(AppDBContext db, IConfiguration configuration)
        {
            _db = db;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }

        public bool IsUniqueUser(string username)
        {
            var user = _db.Users.FirstOrDefault(x => x.UserName == username);
            if (user == null) 
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user = _db.Users.FirstOrDefault(u => u.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && u.Password == loginRequestDTO.Password);

            if (user == null) 
            {
                return null;
            }

            // Generate JWT Token
        }

        public async Task<LocalUser> Register(RegisterationRequestDTO registerationRequestDTO)
        {
            LocalUser user = new()
            {
                UserName = registerationRequestDTO.UserName,
                Password = registerationRequestDTO.Password,
                Name = registerationRequestDTO.Name,
                Role = registerationRequestDTO.Role,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            user.Password = AppConstants.EmptyString;
            return user;
        }
    }
}
