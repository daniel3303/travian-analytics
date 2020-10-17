using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using TravianAnalytics.Models.Identity.Abstract;

namespace TravianAnalytics.Dtos.Identity {
    [AutoMap(typeof(Role))]
    public class RoleDto {
        [Required(ErrorMessage = "O nome é obrigatório.")]
        [MaxLength(16, ErrorMessage = "O nome pode no máximo ter 16 caracteres.")]
        [MinLength(3, ErrorMessage = "O nome deve ter pelo menos 3 caracteres.")]
        public string Name { get; set; }

        public List<ClaimDto> Claims { get; set; }

        public RoleDto() {
            Claims = new List<ClaimDto>();
            foreach (var claimGroup in ClaimStore.ClaimGroups()) {
                foreach (var claim in claimGroup.Claims.OrderBy(c => c.Name)) {
                    Claims.Add(new ClaimDto() {
                        Group = claimGroup.Name,
                        Type = claim.Type,
                        Name = claim.Name,
                        Enabled = false
                    });
                }
            }
        }

        public void SetEnabledClaims(ICollection<RoleClaim> roleClaims) {
            foreach (var claim in Claims.Where(claim => roleClaims.Any(rc => rc.ClaimType == claim.Type))) {
                claim.Enabled = true;
            }
        }

    }

    public class ClaimDto {
        [Required] public string Type { get; set; }
        [Required] public string Name { get; set; }
        [Required] public string Group { get; set; }
        [Required] public bool Enabled { get; set; } = false;
    }
}