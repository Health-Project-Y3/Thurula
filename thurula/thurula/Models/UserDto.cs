namespace thurula.Models;

public class UserDto
{
    public required string Username { get; set; }
    public required string Password { get; set; }
    
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
}