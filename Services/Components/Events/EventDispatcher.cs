using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using TravianAnalytics.Models.Contracts;
using TravianAnalytics.Services.Contracts.Events;

namespace TravianAnalytics.Services.Components.Events {
    public class EventDispatcher : IEventDispatcher {
        private readonly IServiceProvider _serviceContainer;
        private ICollection<IEventHandler> _eventHandlers = null;
        public EventDispatcher(IServiceProvider serviceContainer) {
            _serviceContainer = serviceContainer;
        }

        private void LoadEventHandlers() {
            if (_eventHandlers != null) return;
            _eventHandlers = new List<IEventHandler>();
            var services = _serviceContainer.GetServices<IEventHandler>();
            foreach (var service in services) {
                _eventHandlers.Add(service);
            }
        }

        public async Task DispatchAsync(IDomainEvent domainEvent) {
            LoadEventHandlers();
            foreach (var eventHandler in _eventHandlers) {
                await eventHandler.HandleAsync(domainEvent);
            }
        }
    }
}