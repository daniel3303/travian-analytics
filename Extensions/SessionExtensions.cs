using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace TravianAnalytics.Extensions {
    public static class SessionExtensions {
        public static void SetObject(this ISession session, string key, object value) {
            session.SetString(key, JsonConvert.SerializeObject(value, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            }));
        }
        public static T GetObject<T>(this ISession session, string key) {
            var value = session.GetString(key);
            return value == null ? default : JsonConvert.DeserializeObject<T>(value);
        }

        public static string GetGoBackUrl(this ISession session) {
            var stateDict = session.GetObject<IDictionary<string, string>>("IndexState");
            if (stateDict == null) return session.GetString("IndexPath");
            var target = session.GetObject<IDictionary<string, string>>("IndexState").FirstOrDefault(i => i.Key == session.GetString("Controller")).Value;
            return target ?? session.GetString("IndexPath");
        }
    }
}