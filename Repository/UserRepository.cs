using BackendLaboratory.Data;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

        public bool IsUniqueUser(string email)
        {
            var user = _db.Users.FirstOrDefault(x => x.Email == email);
            if (user == null) 
            {
                return true;
            }
            return false;
        }

        public TokenResponse GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(AppConstants.TokenExpiration),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse()
            {
                Token = tokenHandler.WriteToken(token)
            };
        }

        public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == loginCredentials.Email
            && u.Password == loginCredentials.Password);

            if (user == null) 
            {
                return new TokenResponse()
                {
                    Token = AppConstants.EmptyString
                };
            }

            return GenerateToken(user);
        }

        public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
        {
            User user = new()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                FullName = userRegisterModel.FullName,
                Password = userRegisterModel.Password,
                Email = userRegisterModel.Email,
                BirthDate = userRegisterModel.BirthDate,
                Gender = userRegisterModel.Gender,
                PhoneNumber = userRegisterModel.PhoneNumber,
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return GenerateToken(user);
        }

        public async Task Logout(string userId)
        {
            await Task.CompletedTask;
        }
    }
}
