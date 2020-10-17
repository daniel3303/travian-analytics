using System.Threading.Tasks;

namespace TravianAnalytics.Services.Contracts.CronJobs {
    public interface ICronJobManager {
        public Task ExecuteAsync();
    }
}