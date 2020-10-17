using System.Threading.Tasks;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Services.Contracts.Events {
    public interface IEventDispatcher {
        public Task DispatchAsync(IDomainEvent domainEvent);
    }
}