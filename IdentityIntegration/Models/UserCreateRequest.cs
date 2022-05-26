namespace SecurityIntegration.Models;

public class UserCreateRequest
{
    public string DisplayName { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
