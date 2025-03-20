using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.DTOs;

public class SignupDto
{
    public required string Name { get; set; }
    
    [EmailAddress]
    public required string Email { get; set; }
    
    public required string Password { get; set; }
    
    public required string Role { get; set; }
}