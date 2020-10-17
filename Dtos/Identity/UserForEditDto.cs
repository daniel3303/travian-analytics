using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using AutoMapper;
using TravianAnalytics.Models.Identity.Abstract;

namespace TravianAnalytics.Dtos.Identity {
    [AutoMap(typeof(User))]
    public class UserForEditDto {

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "As passwords não são iguais.")]
        public string RepeatPassword { get; set; }

        [Required(ErrorMessage = "O grupo é obrigatório.")]
        public int GroupId { get; set; }

        public List<ClaimDto> UserClaims { get; set; }

        public UserForEditDto() {
            UserClaims = new List<ClaimDto>();
            foreach (var claimGroup in ClaimStore.ClaimGroups()) {
                foreach (var claim in claimGroup.Claims.OrderBy(c => c.Name)) {
                    UserClaims.Add(new ClaimDto() {
                        Group = claimGroup.Name,
                        Type = claim.Type,
                        Name = claim.Name,
                        Enabled = false
                    });
                }
            }
        }

        public void SetEnabledClaims(ICollection<UserClaim> roleClaims) {
            foreach (var claim in UserClaims.Where(claim => roleClaims.Any(rc => rc.ClaimType == claim.Type))) {
                claim.Enabled = true;
            }
        }

    }
}
