using Microsoft.Extensions.Configuration;
using TravianAnalytics.Data;

namespace TravianAnalytics.Services {
    public class ConfigurationsService {
        private readonly ApplicationDbContext _dbContext;
        public bool RedirectWww { get; }
        public bool RedirectHttps { get; }
        public string Title { get; }
        public string Version { get; }
        
        public ConfigurationsService(ApplicationDbContext dbContext, IConfiguration configuration) {
            _dbContext = dbContext;
            IConfigurationSection configurationSection = configuration.GetSection("Configurations");

            RedirectWww = configurationSection.GetValue<bool>("RedirectWww");
            RedirectHttps = configurationSection.GetValue<bool>("RedirectHttps");
            Title = configurationSection.GetValue<string>("Title");
            Version = configurationSection.GetValue<string>("Version");
        }
    }
}
