using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TravianAnalytics.Dtos.Identity;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Models.Identity.Abstract {
    [AutoMap(typeof(UserSettingsDto))]
    [AutoMap(typeof(UserForEditDto))]
    [Index(nameof(EmailConfirmationToken), IsUnique = true)]
    public class User : IdentityUser<int>, IActivable {
        public override int Id { get; set; }

        [Required]
        [DefaultValue("")]
        private string _name = "";

        public string Name {
            get => _name;
            set {
                _name = value;
                SmallName = "";
                foreach (var name in value.Split(' ')) {
                    SmallName += name[0];
                }
            }
        }

        [Required]
        [DefaultValue("")]
        public string SmallName { get; set; } = "";

        public string EmailConfirmationToken { get; set; }

        public override string PasswordHash { get; set; }
        
        public bool Active { get; set; }

        public bool Master { get; set; }


        public virtual List<UserClaim> Claims { get; set; } = new List<UserClaim>();
        public virtual List<UserLogin> Logins { get; set; } = new List<UserLogin>();
        public virtual List<UserToken> Tokens { get; set; } = new List<UserToken>();

    }

}