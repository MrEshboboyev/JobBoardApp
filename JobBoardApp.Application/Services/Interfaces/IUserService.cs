using JobBoardApp.Application.DTOs;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserDTO>> GetAllUsersAsync();
        Task<UserDTO> GetUserByIdAsync();
        Task<ResponseDTO<bool>> UpdateUserAsync(UserDTO userDto);
        Task<ResponseDTO<bool>> ActivateUserAsync(string userId);
        Task<ResponseDTO<bool>> DeactivateUserAsync(string userId);
        Task<ResponseDTO<bool>> SuspendUserAsync(string userId, string reason, DateTime? endDate);
        Task<ResponseDTO<bool>> DeleteUserAsync(string userId);
        Task<ResponseDTO<bool>> ResetUserPasswordAsync(string userId);
        Task<ResponseDTO<bool>> AssignRoleAsync(string userId, string role);
        Task<ResponseDTO<bool>> UnlockUserAsync(string userId);
        Task<UserActivityDTO> GetUserActivityAsync(string userId);
        Task<IEnumerable<string>> ViewUserRolesAsync(string userId);
    }
}
