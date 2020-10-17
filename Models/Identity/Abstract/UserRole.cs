using Microsoft.AspNetCore.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    public class UserRole : IdentityUserRole<int> {
        public virtual User User { get; set; }
        public virtual Role Role { get; set; }

        public UserRole() {

        }

        public UserRole(User user, Role role) {
            User = user;
            Role = role;
            UserId = user.Id;
            RoleId = role.Id;
        }

        public UserRole(User user, int roleId) {
            User = user;
            UserId = user.Id;
            RoleId = roleId;
        }
    }
}