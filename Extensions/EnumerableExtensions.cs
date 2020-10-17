using System;
using System.Collections.Generic;
using System.Linq;

namespace TravianAnalytics.Extensions {
    public static class EnumerableExtensions {
        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self) => self?.Select((item, index) => (item, index)) ?? new List<(T, int)>();
        public static T GetRandomElement<T>(this IEnumerable<T> self) {
            var enumerable = self as T[] ?? self.ToArray();
            var count = enumerable.Count();
            var rand = new Random();
            return enumerable.ElementAtOrDefault(rand.Next(Math.Max(count, 1)));
        }
    }
}