using Microsoft.AspNetCore.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    public class UserClaim : IdentityUserClaim<int> {

        public virtual string ClaimName { get; set; }
        public virtual User User { get; set; }

        public UserClaim() { }


        public UserClaim(User user, string type, string name) {
            User = user;
            ClaimType = type;
            ClaimValue = type;
            ClaimName = name;
        }
    }
}