using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Jwt;

internal interface IJwtGenerator
{
    string CreateToken(User user);
}
