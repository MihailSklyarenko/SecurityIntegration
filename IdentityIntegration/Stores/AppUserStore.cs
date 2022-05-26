using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using SecurityIntegration.Database.IdentityContext;
using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Stores;

public class AppUserStore : UserStore<User, Role, IdentityContext, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityUserToken<Guid>, IdentityRoleClaim<Guid>>
{
    public AppUserStore(IdentityContext context, IdentityErrorDescriber describer = null) : base(context, describer)
    {
    }
}