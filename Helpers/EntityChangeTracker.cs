using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TravianAnalytics.Data;

namespace TravianAnalytics.Helpers {
    public class EntityChangeTracker {
        private readonly ApplicationDbContext _dbContext;
        private readonly IServiceProvider _serviceProvider;

        public EntityChangeTracker(ApplicationDbContext dbContext, IServiceProvider serviceProvider) {
            _dbContext = dbContext;
            _dbContext.ChangeTracker.StateChanged += OnEntityStateChanged;
            _dbContext.ChangeTracker.Tracked += OnEntityTracked;
            _serviceProvider = serviceProvider;
        }

        public void DisableTracking() {
            _dbContext.ChangeTracker.StateChanged -= OnEntityStateChanged;
            _dbContext.ChangeTracker.Tracked -= OnEntityTracked;
        }

        private void OnEntityStateChanged(object sender, EntityStateChangedEventArgs e) {
        }

        private void OnEntityTracked(object sender, EntityTrackedEventArgs e) {
            
        }
    }
}