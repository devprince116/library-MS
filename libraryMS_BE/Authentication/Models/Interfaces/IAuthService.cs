using Authentication.Models.DTOs;
using Authentication.Models.Entities;

namespace Authentication.Models.Interfaces;

public interface IAuthService
{
    Task<SignupResponse> RegisterAsync(User user);
    Task<AuthResponse> LoginAsync(string email, string password);
}