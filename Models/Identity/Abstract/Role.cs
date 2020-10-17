using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using TravianAnalytics.Dtos.Identity;

namespace TravianAnalytics.Models.Identity.Abstract {
    [AutoMap(typeof(RoleDto))]
    public class Role : IdentityRole<int> {
        public virtual string Description { get; set; }
        public virtual ICollection<UserRole> UserRoles { get; set; }
        public virtual ICollection<RoleClaim> RoleClaims { get; set; }

        public Role() {
            RoleClaims = new List<RoleClaim>();
        }
    }
}