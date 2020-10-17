using System;

namespace TravianAnalytics.Extensions {
    public static class TimeSpanExtensions {
        
        /**
         * <summary>TimeSpan to human string</summary>
         */
        public static string Humanize(this TimeSpan timeSpan) {
            if (timeSpan.Days > 0) {
                return $"{timeSpan.Days} dias, {timeSpan.Hours} horas e {timeSpan.Minutes} minutos";
            }
            if (timeSpan.Hours > 0) {
                return $"{timeSpan.Hours} horas e {timeSpan.Minutes} minutos";
            }
            if (timeSpan.Minutes > 0) {
                return $"{timeSpan.Minutes} minutos";
            }

            return "há menos de um minuto";

        }
    }
}
