namespace OrderProductApi.Responses;

public class AuthResponse
{
    public int UserId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty;
    public string Token { get; set; } = string.Empty;

    public AuthResponse(int userId, string name, string email, string role, string token)
    {
        UserId = userId;
        Name = name;
        Email = email;
        Role = role;
        Token = token;
    }
}
