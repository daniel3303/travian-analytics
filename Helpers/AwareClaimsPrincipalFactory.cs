using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using TravianAnalytics.Models.Identity;
using TravianAnalytics.Models.Identity.Abstract;

namespace TravianAnalytics.Helpers {
    public class AwareClaimsPrincipalFactory : UserClaimsPrincipalFactory<User> {
        public AwareClaimsPrincipalFactory(UserManager<User> userManager,
            IOptions<IdentityOptions> optionsAccessor) :
            base(userManager, optionsAccessor) { }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(User user) {
            var identity = await base.GenerateClaimsAsync(user);

            // Add the claims to the user
            foreach (var claim in user.Claims) {
                identity.AddClaim(new Claim(claim.ClaimType, claim.ClaimName));
            }

            // If the user is an Employee
            if (user.Master) {
                Console.WriteLine("here");
                identity.AddClaim(ClaimStore.MasterClaim);
                foreach (var claim in ClaimStore.ClaimList()
                    .Where(claim => !identity.HasClaim(c => c.Value == claim.Value))) {
                    identity.AddClaim(claim);
                }
            }

            return identity;
        }


    }
}