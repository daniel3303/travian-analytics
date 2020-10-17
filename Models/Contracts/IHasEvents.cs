using System.Collections.Generic;

namespace TravianAnalytics.Models.Contracts {
    public interface IHasEvents {
        ICollection<IDomainEvent> Events { get; }
    }
}