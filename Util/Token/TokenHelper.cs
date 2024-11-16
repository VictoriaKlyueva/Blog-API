using BackendLaboratory.Data.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BackendLaboratory.Util.Token
{
    public class TokenHelper
    {
        private readonly string _secretKey;
        private JwtSecurityTokenHandler _tokenHandler;

        public TokenHelper(IConfiguration configuration)
        {
            _secretKey = configuration.GetValue<string>("ApiSettings:Secret");
            _tokenHandler = new JwtSecurityTokenHandler();
        }

        public TokenResponse GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Sid, user.Id.ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(AppConstants.TokenExpiration),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new TokenResponse
            {
                Token = tokenHandler.WriteToken(token)
            };
        }

        public string GetIdFromToken(string token)
        {
            var jwtToken = _tokenHandler.ReadToken(token) as JwtSecurityToken;
            if (jwtToken == null)
            {
                Console.WriteLine(jwtToken);
            }

            var id = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Sid).Value;

            return id;
        }
    }
}
