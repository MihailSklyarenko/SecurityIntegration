using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SecurityIntegration.Database.IdentityContext;
using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Stores;

public class AppRoleStore : RoleStore<Role, IdentityContext, Guid, IdentityUserRole<Guid>, IdentityRoleClaim<Guid>>
{
    public AppRoleStore(IdentityContext context, IdentityErrorDescriber describer = null) : base(context, describer)
    {
    }
}