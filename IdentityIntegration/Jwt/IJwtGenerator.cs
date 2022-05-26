using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Jwt;

public interface IJwtGenerator
{
    string CreateToken(User user);
}
