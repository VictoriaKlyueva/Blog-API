﻿using BackendLaboratory.Data;
using BackendLaboratory.Data.Entities;
using BackendLaboratory.Repository.IRepository;
using BackendLaboratory.Util;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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

            return _tokenHelper.GenerateToken(user);
        }

        public async Task Logout(string userId)
        {
            await Task.CompletedTask;
        }
    }
}
