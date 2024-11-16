﻿using BackendLaboratory.Constants;
using BackendLaboratory.Data;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util;
using BackendLaboratory.Util.Validators;

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
            var user = _db.Users.FirstOrDefault(u => u.Email == loginCredentials.Email
            && u.Password == loginCredentials.Password);

            if (user == null) 
            {
                return new TokenResponse()
                {
                    Token = AppConstants.EmptyString
                };
            }

            return _tokenHelper.GenerateToken(user);
        }

        private void ValidateUserRegistration(UserRegisterModel userRegisterModel)
        {
            if (!RegisterValidator.IsEmailValid(userRegisterModel.Email))
            {
                throw new Exception(ErrorMessages.InvalidEmail);
            }
            if (!RegisterValidator.IsBirthDateValid(userRegisterModel.BirthDate))
            {
                throw new Exception(ErrorMessages.InvalidBirthDate);
            }
            if (!RegisterValidator.IsFullnameValid(userRegisterModel.FullName))
            {
                throw new Exception(ErrorMessages.InvalidFullName);
            }
            if (!RegisterValidator.IsPasswordStrong(userRegisterModel.Password))
            {
                throw new Exception(ErrorMessages.WeakPassword);
            }
            if (!RegisterValidator.IsPhoneValid(userRegisterModel.PhoneNumber))
            {
                throw new Exception(ErrorMessages.InvalidPhoneNumber);
            }
        }

        public async Task<TokenResponse> Register(UserRegisterModel userRegisterModel)
        {
            ValidateUserRegistration(userRegisterModel);

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

            return _tokenHelper.GenerateToken(user);
        }

        public async Task Logout(string token)
        {
            string id = _tokenHelper.GetIdFromToken(token);

            if (Guid.TryParse(id, out Guid doctorId) && doctorId != Guid.Empty)
            {
                await _db.BlackTokens.AddAsync(new BlackToken { Blacktoken = token });
                await _db.SaveChangesAsync();
            }
            else
            {
                throw new Exception("Некорректный ID: не удалось извлечь или преобразовать id из токена.");
            }
        }
    }
}
