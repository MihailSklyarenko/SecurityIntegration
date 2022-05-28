using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Managers;

public class AppRoleManager : RoleManager<Role>
{
    public AppRoleManager(IRoleStore<Role> store,
        IEnumerable<IRoleValidator<Role>> roleValidators,
        ILookupNormalizer keyNormalizer,
        IdentityErrorDescriber errors,
        ILogger<AppRoleManager> logger)
        : base(store, roleValidators, keyNormalizer, errors, logger)
    {
    }
}