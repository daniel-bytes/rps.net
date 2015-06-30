using System.Data.Entity;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web;
using System.Security.Principal;

namespace Rps.Web.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit http://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }

        public static Domain.Model.Player GetCurrentPlayer(HttpContextBase context)
        {
            return GetCurrentPlayer(context.User);
        }

        public static Domain.Model.Player GetCurrentPlayer(IPrincipal principal)
        {
            var identity = principal.Identity as ClaimsIdentity;

            if (identity != null)
            {
                var idClaim = identity.Claims.SingleOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                if (idClaim != null)
                {
                    return new Domain.Model.Player(idClaim.Value, identity.Name, false);
                }
            }

            return Domain.Model.Player.Empty;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base(Rps.Domain.Config.DomainConfig.ConnStringKey_DefaultConnection, throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}