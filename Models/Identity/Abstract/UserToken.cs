using Microsoft.AspNetCore.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    public class UserToken : IdentityUserToken<int> {
        public virtual User User { get; set; }
    }
}