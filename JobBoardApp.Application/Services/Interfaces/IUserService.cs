using JobBoardApp.Application.DTOs;
using JobBoardApp.Application.Requests;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<ResponseDTO<IEnumerable<UserDTO>>> GetAllUsersAsync();
        Task<ResponseDTO<UserDTO>> GetUserByIdAsync(string userId);
        Task<ResponseDTO<bool>> UpdateUserAsync(UserDTO userDto);
        Task<ResponseDTO<bool>> ActivateUserAsync(string userName);
        Task<ResponseDTO<bool>> DeactivateUserAsync(string userName);
        Task<ResponseDTO<bool>> SuspendUserAsync(SuspendUserRequest suspendUserRequest);
        Task<ResponseDTO<bool>> UnsuspendUserAsync(string userName);
        Task<ResponseDTO<bool>> DeleteUserAsync(string userId);
        Task<ResponseDTO<bool>> ResetUserPasswordAsync(string userId, string newPassword);
        Task<ResponseDTO<bool>> AssignRoleAsync(AssignRoleRequest request);
        Task<ResponseDTO<bool>> UnlockUserAsync(string userId);
        Task<ResponseDTO<UserActivityDTO>> GetUserActivityAsync(string userId);
        Task<ResponseDTO<IEnumerable<string>>> ViewUserRolesAsync(string userId);
    }
}
