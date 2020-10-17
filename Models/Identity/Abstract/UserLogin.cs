using Microsoft.AspNetCore.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    public class UserLogin : IdentityUserLogin<int> {
        public virtual User User { get; set; }
    }
}