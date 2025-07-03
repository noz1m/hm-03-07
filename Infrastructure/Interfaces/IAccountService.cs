using Domain.ApiResponse;
using Domain.DTOs.Account;

namespace Infrastructure.Interfaces;

public interface IAccountService
{
    Task<Response<string>> RegisterAsync(RegisterDTO register);
    Task<Response<string>> LoginAsync(LoginDTO login);
    Task<Response<string>> LogoutAsync();
}
