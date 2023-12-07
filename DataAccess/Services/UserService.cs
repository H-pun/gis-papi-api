﻿using Microsoft.EntityFrameworkCore;
using GISPaPi.Base;
using GISPaPi.DataAccess.Entities;
using GISPaPi.DataAccess.Models;
using GISPaPi.Helpers;
using System.Net;
using System.Security.Claims;

namespace GISPaPi.DataAccess.Services
{
    public interface IUserService : IBaseService<User>
    {
        Task<LoginResponse> Login(string username, string password);
        Task ChangePassword(ChangePasswordRequest model);
    }
    public class UserService : BaseService<User>, IUserService
    {
        public UserService(AppDbContext appDbContext) : base(appDbContext) { }
        public async Task<LoginResponse> Login(string username, string password)
        {
            User user = await _appDbContext.Set<User>().FirstOrDefaultAsync(x => x.Username == username);
            LoginResponse loginResponse = new();

            if (user != null)
            {
                if (!PasswordHelper.VerifyHashedPassword(user.Password, password))
                    throw new HttpRequestException("Wrong password!", null, HttpStatusCode.Unauthorized);

                List<Claim> claims = new()
                {
                    new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new(ClaimTypes.Role, user.Role.ToString())
                };
                
                user.AppToken = JwtHelper.CreateToken(claims.ToArray(), 72);
                loginResponse.MapToModel(user);
                
                await _appDbContext.SaveChangesAsync();
            }
            else
                throw new HttpRequestException("Username not found!", null, HttpStatusCode.NotFound);

            return loginResponse;
        }
        public async Task ChangePassword(ChangePasswordRequest model)
        {
            User user = await _appDbContext.Set<User>().FindAsync(model.IdUser);
            if (!PasswordHelper.VerifyHashedPassword(user.Password, model.OldPassword))
                throw new HttpRequestException("Wrong old password!", null, HttpStatusCode.Unauthorized);
            user.Password = model.NewPassword.HashPassword();
            await _appDbContext.SaveChangesAsync();
        }
    }
}
