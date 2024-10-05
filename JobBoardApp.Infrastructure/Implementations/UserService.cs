﻿using AutoMapper;
using JobBoardApp.Application.Common.Interfaces;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Services.Interfaces;
using JobBoardApp.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace JobBoardApp.Infrastructure.Implementations
{
    public class UserService(UserManager<AppUser> userManager,
        RoleManager<IdentityRole> roleManager,
        IMapper mapper,
        IUnitOfWork unitOfWork) : IUserService
    {
        private readonly UserManager<AppUser> _userManager = userManager;
        private readonly RoleManager<IdentityRole> _roleManager = roleManager;
        private readonly IMapper _mapper = mapper;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ResponseDTO<IEnumerable<UserDTO>>> GetAllUsersAsync()
        {
            try
            {
                var allUsers = await _unitOfWork.User.GetAllAsync();
                var userDTOs = new List<UserDTO>();

                foreach (var user in allUsers)
                {
                    var mappedUser = _mapper.Map<UserDTO>(user);
                    var roles = await _userManager.GetRolesAsync(user);
                    mappedUser.Roles = roles; // Assign roles after mapping
                    userDTOs.Add(mappedUser);
                }

                return new ResponseDTO<IEnumerable<UserDTO>>(userDTOs);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<UserDTO>>(ex.Message);
            }
        }


        public async Task<ResponseDTO<UserDTO>> GetUserByIdAsync(string userId)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                var mappedUser = _mapper.Map<UserDTO>(userFromDb);
                var roles = await _userManager.GetRolesAsync(userFromDb);
                mappedUser.Roles = roles; // Assign roles after mapping

                return new ResponseDTO<UserDTO>(mappedUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserDTO>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UpdateUserAsync(UserDTO userDTO)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userDTO.Id)
                    ) ?? throw new Exception("User not found!");

                // mapping fields
                _mapper.Map(userDTO, userFromDb);

                await _unitOfWork.User.UpdateAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> ActivateUserAsync(string userName)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.UserName.Equals(userName)
                    ) ?? throw new Exception("User not found!");

                // update activate field
                userFromDb.IsActive = true;

                await _unitOfWork.User.UpdateAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> DeactivateUserAsync(string userName)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.UserName.Equals(userName)
                    ) ?? throw new Exception("User not found!");

                // update activate field
                userFromDb.IsActive = false;

                await _unitOfWork.User.UpdateAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> SuspendUserAsync(string userId, string reason, DateTime? endDate)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                // update suspension fields
                userFromDb.SuspensionReason = reason;
                userFromDb.SuspensionEndDate = endDate;

                await _unitOfWork.User.UpdateAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> DeleteUserAsync(string userId)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                await _unitOfWork.User.RemoveAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> ResetUserPasswordAsync(string userId, string newPassword)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                // Generate reset token
                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(userFromDb);

                var result = await _userManager.ResetPasswordAsync(userFromDb, resetToken, newPassword);
                
                if (!result.Succeeded)
                {
                    // Collect errors if resetting password fails
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception(result.Errors.First().Description);
                }

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> AssignRoleAsync(string userId, string role)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                var result = await _userManager.AddToRoleAsync(userFromDb, role);
                if (!result.Succeeded)
                    throw new Exception(result.Errors.First().Description);

                await _unitOfWork.User.UpdateAsync(userFromDb);
                await _unitOfWork.SaveAsync();

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<bool>> UnlockUserAsync(string userId)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                if (userFromDb.LockoutEnd is not null || userFromDb.LockoutEnd <= DateTime.Now)
                    throw new Exception("User is not locked out!");

                var result = await _userManager.SetLockoutEndDateAsync(userFromDb, null);

                if (result.Succeeded)
                {
                    // Collect errors if resetting password fails
                    var errorMessage = string.Join(", ", result.Errors.Select(e => e.Description));
                    throw new Exception(result.Errors.First().Description);
                }

                return new ResponseDTO<bool>(true);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<bool>(ex.Message);
            }
        }

        public async Task<ResponseDTO<UserActivityDTO>> GetUserActivityAsync(string userId)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    filter: u => u.Id.Equals(userId),
                    includeProperties: "JobApplications,JobListings"
                    ) ?? throw new Exception("User not found!");

                var mappedUser = _mapper.Map<UserActivityDTO>(userFromDb);

                return new ResponseDTO<UserActivityDTO>(mappedUser);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<UserActivityDTO>(ex.Message);
            }
        }

        public async Task<ResponseDTO<IEnumerable<string>>> ViewUserRolesAsync(string userId)
        {
            try
            {
                var userFromDb = await _unitOfWork.User.GetAsync(
                    u => u.Id.Equals(userId)
                    ) ?? throw new Exception("User not found!");

                var roles = await _userManager.GetRolesAsync(userFromDb);

                return new ResponseDTO<IEnumerable<string>>(roles);
            }
            catch (Exception ex)
            {
                return new ResponseDTO<IEnumerable<string>>(ex.Message);
            }
        }
    }
}
