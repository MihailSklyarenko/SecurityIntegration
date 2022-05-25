using Microsoft.AspNetCore.Identity;

namespace SecurityIntegration.Database.IdentityEntries;

public class User : IdentityUser<Guid>
{
    public string DisplayName { get; set; } 
}
