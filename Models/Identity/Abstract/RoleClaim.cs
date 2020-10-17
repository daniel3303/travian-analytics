using Microsoft.AspNetCore.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    public class RoleClaim : IdentityRoleClaim<int> {
        public virtual string ClaimName { get; set; }
        public virtual Role Role { get; set; }

        public RoleClaim() { }

        public RoleClaim(string type, string name) {
            ClaimType = type;
            ClaimValue = type;
            ClaimName = name;
        }
    }
}