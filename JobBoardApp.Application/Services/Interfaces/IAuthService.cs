using JobBoardApp.Application.Common.Models;
using JobBoardApp.Application.DTOs;
using JobBoardApp.Domain.Entities;

namespace JobBoardApp.Application.Services.Interfaces
{
    public interface IAuthService
    {
        Task<ResponseDTO<string>> LoginAsync(LoginModel loginModel);
        Task<ResponseDTO<string>> RegisterAsync(RegisterModel registerModel);
        Task<ResponseDTO<string>> GenerateJwtToken(AppUser user, IEnumerable<string> roles);

        Task<ResponseDTO<bool>> EmailExist(string email);
        Task<ResponseDTO<bool>> UserNameExist(string username);
    }
}
