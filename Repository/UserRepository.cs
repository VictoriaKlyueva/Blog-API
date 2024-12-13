using BackendLaboratory.Constants;
using BackendLaboratory.Data.Database;
using BackendLaboratory.Data.DTO;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util.CustomExceptions.Exceptions;
using BackendLaboratory.Util.Password;
using BackendLaboratory.Util.Token;
using BackendLaboratory.Util.Validators;
using Microsoft.EntityFrameworkCore;
using System.Numerics;

namespace BackendLaboratory.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDBContext _db;
        private readonly TokenHelper _tokenHelper;

        public UserRepository(AppDBContext db, IConfiguration configuration)
        {
            _db = db;
            _tokenHelper = new TokenHelper(configuration);
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

        public async Task<TokenResponse> Login(LoginCredentials loginCredentials)
        {
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Email == loginCredentials.Email);

            if (user == null || !HashingPassword.VerifyPassword(loginCredentials.Password, user.Password))
            {
                return new TokenResponse()
                {
                    Token = AppConstants.EmptyString
                };
            }

            return _tokenHelper.GenerateToken(user);
        }

        private void ValidateUser(UserRegisterModel userRegisterModel)
        {
            if (!IsUniqueUser(userRegisterModel.Email))
            {
                throw new BadRequestException(ErrorMessages.UserIsAlreadyExcist);
            }
            if (!RegisterValidator.IsEmailValid(userRegisterModel.Email))
            {
                throw new BadRequestException(ErrorMessages.InvalidEmail);
            }
            if (!RegisterValidator.IsBirthDateValid(userRegisterModel.BirthDate))
            {
                throw new BadRequestException(ErrorMessages.InvalidBirthDate);
            }
            if (!RegisterValidator.IsFullnameValid(userRegisterModel.FullName))
            {
                throw new BadRequestException(ErrorMessages.InvalidFullName);
            }
            if (!RegisterValidator.IsPasswordStrong(userRegisterModel.Password))
            {
                throw new BadRequestException(ErrorMessages.WeakPassword);
            }
            if (!RegisterValidator.IsPhoneValid(userRegisterModel.PhoneNumber))
            {
                throw new BadRequestException(ErrorMessages.InvalidPhoneNumber);
            }
        }

        private void ValidateUser(UserEditModel userEditModel)
        {
            if (!RegisterValidator.IsEmailValid(userEditModel.Email))
            {
                throw new BadRequestException(ErrorMessages.InvalidEmail);
            }
            if (!RegisterValidator.IsBirthDateValid(userEditModel.BirthDate))
            {
                throw new BadRequestException(ErrorMessages.InvalidBirthDate);
            }
            if (!RegisterValidator.IsFullnameValid(userEditModel.FullName))
            {
                throw new BadRequestException(ErrorMessages.InvalidFullName);
            }
            if (!RegisterValidator.IsPhoneValid(userEditModel.PhoneNumber))
            {
                throw new BadRequestException(ErrorMessages.InvalidPhoneNumber);
            }
        }

        public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
        {
            ValidateUser(userRegisterModel);

            User user = new()
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                FullName = userRegisterModel.FullName,
                Password = HashingPassword.HashPassword(userRegisterModel.Password),
                Email = userRegisterModel.Email,
                BirthDate = userRegisterModel.BirthDate,
                Gender = userRegisterModel.Gender,
                PhoneNumber = userRegisterModel.PhoneNumber
            };

            _db.Users.Add(user);
            await _db.SaveChangesAsync();

            return _tokenHelper.GenerateToken(user);
        }

        public async Task Logout(string token)
        {
            var id = _tokenHelper.GetIdFromToken(token);

            if (Guid.TryParse(id, out Guid doctorId) && doctorId != Guid.Empty)
            {
                await _db.BlackTokens.AddAsync(new BlackToken { Blacktoken = token });
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new BadRequestException(ErrorMessages.IncorrectId);
            }
        }

        public async Task<UserDto> GetProfile(string token)
        {
            var  userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);

            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            return new UserDto
            {
                Id = Guid.NewGuid(),
                CreateTime = DateTime.UtcNow,
                FullName = user.FullName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public async Task EditProfile(string token, UserEditModel UserEditModel)
        {
            ValidateUser(UserEditModel);

            var userId = _tokenHelper.GetIdFromToken(token);
            var user = await _db.Users.FirstOrDefaultAsync(u => u.Id.ToString() == userId);
            if (user != _db.Users.FirstOrDefault(x => x.Email == UserEditModel.Email))
            {
                throw new BadRequestException(ErrorMessages.UserIsAlreadyExcist);
            }

            if (user == null) { throw new UnauthorizedException(ErrorMessages.ProfileNotFound); }

            user.Email = UserEditModel.Email;
            user.FullName = UserEditModel.FullName;
            user.BirthDate = UserEditModel.BirthDate;
            user.Gender = UserEditModel.Gender;
            user.PhoneNumber = UserEditModel.PhoneNumber;

            await _db.SaveChangesAsync();
        }
    }
}
