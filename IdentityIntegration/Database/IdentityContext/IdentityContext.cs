using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SecurityIntegration.Database.IdentityEntries;

namespace SecurityIntegration.Database.IdentityContext;

public class IdentityContext : IdentityDbContext<User, Role, Guid, IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>, IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>
{
    public IdentityContext(DbContextOptions<IdentityContext> options) : base(options) {  }
}