﻿using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.Common.Models;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class AuthService(UserManager<AppUser> userManager,
        SignInManager<AppUser> signInManager,
        IConfiguration config,
        IUnitOfWork unitOfWork) : IAuthService
    {
        // inject Identity Managers
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly SignInManager<AppUser> _signInManager = signInManager;
        private readonly IConfiguration _config = config;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private const int _expirationTokenHours = 12;

        public async Task<ResponseDTO<string>> GenerateJwtToken(AppUser user, IEnumerable<string> roles)
        {
            try
            {
                // create token handler instance
                var tokenHandler = new JwtSecurityTokenHandler();

                var key = Encoding.ASCII.GetBytes(_config["JwtSettings:Key"]);
                var issuer = _config["JwtSettings:Issuer"];
                var audience = _config["JwtSettings:Audience"];

                var claimList = new List<Claim>
                {
                    new(JwtRegisteredClaimNames.Sub, user.Id),
                    new(JwtRegisteredClaimNames.Email, user.Email)
                };

                // adding roles to claim List
                claimList.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

                // signing credentials
                var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    IssuedAt = DateTime.UtcNow,
                    Issuer = issuer,
                    Audience = audience,
                    Expires = DateTime.UtcNow.AddHours(_expirationTokenHours),
                    Subject = new ClaimsIdentity(claimList),
                    SigningCredentials = signingCredentials
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                return new ResponseDTO<string>(tokenString, "Token generated successfully!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> LoginAsync(LoginModel loginModel)
        {
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginModel.UserName,
                    loginModel.Password, false, false);

                if (!result.Succeeded)
                    return new ResponseDTO<string>("Email/Password is incorrect!");

                // getting this user and user roles
                var userFromDb = await _userManager.FindByNameAsync(loginModel.UserName)
                    ?? throw new Exception("User not found in our system!");

                var userRoles = await _userManager.GetRolesAsync(userFromDb);

                // create and return token
                var generatedToken = await GenerateJwtToken(userFromDb, userRoles);

                return new ResponseDTO<string>(generatedToken.Data, "Login successfully!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>(ex.Message);
            }
        }

        public async Task<ResponseDTO<string>> RegisterAsync(RegisterModel registerModel)
        {
            try
            {
                var emailExist = await EmailExist(registerModel.Email);
                var usernameExist = await UserNameExist(registerModel.UserName);

                if (emailExist.Data || usernameExist.Data)
                    return new ResponseDTO<string>("Username/email already exist!");

                // create new AppUser instance
                AppUser user = new()
                {
                    Email = registerModel.Email,
                    UserName = registerModel.UserName,
                    DateRegistered = DateTime.Now 
                };

                
                // create and added to db user
                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if(!result.Succeeded)
                    return new ResponseDTO<string>($"Error : {result.Errors.FirstOrDefault()!.Description}");

                // assign role
                await _userManager.AddToRoleAsync(user, registerModel.Role.ToString());

                // create user profile
                UserProfile userProfile = new()
                {
                    Bio = registerModel.Bio,
                    CompanyName = registerModel.CompanyName,
                    Website = registerModel.Website,
                    UserId = user.Id,
                    ResumePath = registerModel.ResumePath
                };

                await _unitOfWork.UserProfile.AddAsync(userProfile);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<string>(null, "Registration successful!");
            }
            catch (Exception ex)
            {
                return new ResponseDTO<string>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> EmailExist(string email)
        {
            try
            {
                var userExist = await _userManager.FindByEmailAsync(email);

                if (userExist is null)
                    return new ResponseDTO<bool>(false);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UserNameExist(string username)
        {
            try
            {
                var userExist = await _userManager.FindByNameAsync(username);

                if (userExist is null)
                    return new ResponseDTO<bool>(false);

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }
    }
}
