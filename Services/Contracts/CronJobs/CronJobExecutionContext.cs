using System;

namespace TravianAnalytics.Services.Contracts.CronJobs {
    public class CronJobExecutionContext {
        public DateTime LastExecutionTime { get; }
        public TimeSpan IntervalSinceLastExecution { get; }

        public CronJobExecutionContext(DateTime lastExecutionTime, TimeSpan intervalSinceLastExecution) {
            LastExecutionTime = lastExecutionTime;
            IntervalSinceLastExecution = intervalSinceLastExecution;
        }
    }
}