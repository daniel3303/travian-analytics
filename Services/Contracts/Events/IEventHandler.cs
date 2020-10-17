using System.Threading.Tasks;
using TravianAnalytics.Models.Contracts;

namespace TravianAnalytics.Services.Contracts.Events {
    public interface IEventHandler {
        public Task HandleAsync(IDomainEvent domainEvent);
    }
}