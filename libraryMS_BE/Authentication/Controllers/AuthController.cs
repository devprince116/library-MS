using Authentication.Models.DTOs; 
using Authentication.Models.Entities;
using Authentication.Models.Interfaces;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] SignupDto request)
    {
        try
        {
            var user = new User
            {
                Name = request.Name,
                Email = request.Email,
                Password = request.Password, 
                Role = request.Role
            };
            var response = await _authService.RegisterAsync(user);
            return Ok(new { message = "User created successfully!", Response = response });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDto request)
    {
        try
        {
            var response = await _authService.LoginAsync(request.Email, request.Password);
            return Ok(new { message = "Login successful", Response = response });
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}

