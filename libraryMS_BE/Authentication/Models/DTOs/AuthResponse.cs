namespace Authentication.Models.DTOs;

public class AuthResponse
{
    public string Token { get; set; }
    public string Role { get; set; }
    public Guid UserId { get; set; }
}

public class SignupResponse
{   public string Name { get; set; }
    public string Email { get; set; }
    public Guid UserId { get; set; }
    public string Role { get; set; }
}