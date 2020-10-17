using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravianAnalytics.Data;
using TravianAnalytics.Models.CronJob;
using TravianAnalytics.Services.Contracts.CronJobs;

namespace TravianAnalytics.Services.Components.CronJobs {
    public class CronJobManager : ICronJobManager {
        private readonly IServiceProvider _serviceContainer;
        private readonly ApplicationDbContext _dbContext;
        private ICollection<ICronJobSubscriber> _cronJobSubscribers;
        public CronJobManager(IServiceProvider serviceContainer, ApplicationDbContext dbContext) {
            _serviceContainer = serviceContainer;
            _dbContext = dbContext;
        }

        private void LoadCronJobSubscribers() {
            if (_cronJobSubscribers != null) return;
            _cronJobSubscribers = new List<ICronJobSubscriber>();
            var services = _serviceContainer.GetServices<ICronJobSubscriber>();
            foreach (var service in services) {
                _cronJobSubscribers.Add(service);
            }
        }

        public async Task ExecuteAsync() {
            LoadCronJobSubscribers();
            foreach (var cronJobSubscriber in _cronJobSubscribers) {
                var now = DateTime.Now;
                var lastCronJob = await _dbContext.CronJobs
                    .Where(cj => cj.Service == cronJobSubscriber.GetType().FullName)
                    .OrderByDescending(cj => cj.ExecutionTime).FirstOrDefaultAsync();
                var lastExecutionTime = lastCronJob?.ExecutionTime ?? DateTime.MinValue;
                var timeSinceLastExecution = now - lastExecutionTime;
                if(timeSinceLastExecution < cronJobSubscriber.GetMinimumIntervalBetweenExecutions()) continue;
                var executionContext = new CronJobExecutionContext(lastExecutionTime, timeSinceLastExecution);
                var executionResult = new CronJob() {
                    Service = cronJobSubscriber.GetType().FullName,
                    Success = true,
                    ExecutionTime = now
                };
                try {
                    await cronJobSubscriber.ExecuteAsync(executionContext);
                } catch (Exception e) {
                    executionResult.Success = false;
                    executionResult.Output = e.Message;
                }

                _dbContext.Add(executionResult);
            }

            await _dbContext.SaveChangesAsync();
        }
    }
}