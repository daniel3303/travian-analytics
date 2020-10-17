using System;

namespace TravianAnalytics.Extensions {
    public static class TimeSpanExtensions {
        
        /**
         * <summary>TimeSpan to human string</summary>
         */
        public static string Humanize(this TimeSpan timeSpan) {
            if (timeSpan.Days > 0) {
                return $"{timeSpan.Days} dias, {timeSpan.Hours}h e {timeSpan.Minutes}m";
            }
            if (timeSpan.Hours > 0) {
                return $"{timeSpan.Hours}h e {timeSpan.Minutes}m";
            }
            if (timeSpan.Minutes > 0) {
                return $"{timeSpan.Minutes}m";
            }

            return "há menos de um minuto";

        }
    }
}
