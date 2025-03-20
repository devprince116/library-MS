using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Authentication.Models.DTOs;
using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Authentication.Services;

public class AuthService:  IAuthService
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _configuration;

    public AuthService(AppDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<SignupResponse> RegisterAsync(User user)
    {
 
        if (await _context.Users.AnyAsync(u => u.Email == user.Email))
            throw new Exception("Email already exists");

        if (user.Role != "Admin" && user.Role != "User")
            throw new Exception("Invalid role. Must be 'Admin' or 'User'");
        
        user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);

        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        return new SignupResponse
        {
            Name = user.Name,
            Email = user.Email,
            Role = user.Role,
            UserId = user.Id
        };
    }

    public async Task<AuthResponse> LoginAsync(string email, string password)
    {
        var user = await _context.Users
            .FirstOrDefaultAsync(u => u.Email == email);

        if (user == null || !BCrypt.Net.BCrypt.Verify(password, user.Password))
            throw new Exception("Invalid email or password");

        var token = GenerateJwtToken(user);
        return new AuthResponse
        {
            Token = token,
            Role = user.Role,
            UserId = user.Id
        };
    }

    private string GenerateJwtToken(User user)
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.Role, user.Role),
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Secret"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _configuration["Jwt:ValidIssuer"],
            audience: _configuration["Jwt:ValidAudience"],
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}