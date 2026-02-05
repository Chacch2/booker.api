using booker.api.Models;
using booker.api.Models.DTOs;

namespace booker.api.Services.Interface
{
    public interface IAuthService
    {
        Task<ApiResponse> RegisterAsync(RegisterRequestDTO registerRequestDTO);
        Task<ApiResponse> LoginAsync(LoginRequestDTO loginRequestDTO);
    }
}
