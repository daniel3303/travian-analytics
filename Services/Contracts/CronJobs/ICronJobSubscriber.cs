using System;
using System.Threading.Tasks;

namespace TravianAnalytics.Services.Contracts.CronJobs {
    public interface ICronJobSubscriber {
        public TimeSpan GetMinimumIntervalBetweenExecutions();
        
        
        /**
         * <summary>
         * Define this function to execute your code every period of time
         * You can throw exception inside
         * </summary>
         */
        public Task ExecuteAsync(CronJobExecutionContext context);
    }
}