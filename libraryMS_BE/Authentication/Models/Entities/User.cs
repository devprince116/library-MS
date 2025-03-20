using System.ComponentModel.DataAnnotations;

namespace Authentication.Models.Entities;

public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public required string Name { get; set; }
    [EmailAddress]
    public required string Email { get; set; }
    public required string Password { get; set; }
    public required string Role { get; set; }
}