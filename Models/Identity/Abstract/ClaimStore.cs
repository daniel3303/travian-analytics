using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace TravianAnalytics.Models.Identity.Abstract {
    public static class ClaimStore {
        private static readonly List<ApplicationClaimGroup> Claims = new List<ApplicationClaimGroup> {
            new ApplicationClaimGroup("Relatórios",
                new[] {
                    new ApplicationClaim("Alliances", "Alliances", "Alianças"),
                    new ApplicationClaim("Players", "Players", "Jogadores"),
                    //new ApplicationClaim("CronJobs", "Master", "Tarefas Agendadas")
                }
            ),
            new ApplicationClaimGroup("A minha conta",
                new[] {
                    new ApplicationClaim("ChangePassword", "", "Alterar Password"),
                }
            ),
        };

        public static readonly Claim MasterClaim = new Claim("Master", "Master");

        public static List<ApplicationClaimGroup> ClaimGroups() {
            return Claims;
        }

        public static List<ApplicationClaim> ClaimList() {
            var claimsList = new List<ApplicationClaim>();
            foreach (var claimGroup in Claims) {
                foreach (var claim in claimGroup.Claims) {
                    claimsList.Add(new ApplicationClaim(claim.ControllerName, claim.Type, claim.Name));
                }
            }

            return claimsList;
        }
    }

    public class ApplicationClaimGroup {
        public string Name { get; set; }
        private readonly Func<IServiceProvider, Task<string>> _nameFunc;
        public ApplicationClaim[] Claims { get; set; }
        public Type UserType { get; }

        public ApplicationClaimGroup(string name, ApplicationClaim[] claims, Type userType = null,
            Func<IServiceProvider, Task<string>> nameFunc = null) {
            Name = name;
            Claims = claims;
            _nameFunc = nameFunc;
            UserType = userType ?? typeof(User);
        }

        public async Task<string> GetComputedName(IServiceProvider serviceProvider) {
            return _nameFunc != null ? await _nameFunc(serviceProvider) ?? Name : Name;
        }
    }

    public class ApplicationClaim : Claim {
        public string Name { get; }
        private readonly Func<IServiceProvider, Task<string>> _nameFunc;
        public string ControllerName { get; }

        public ApplicationClaim(string controllerName, string type, string name,
            Func<IServiceProvider, Task<string>> nameFunc = null) : base(type, type) {
            Name = name;
            _nameFunc = nameFunc;
            ControllerName = controllerName;
        }

        public async Task<string> GetComputedName(IServiceProvider serviceProvider) {
            return _nameFunc != null ? await _nameFunc(serviceProvider) ?? Name : Name;
        }
    }
}