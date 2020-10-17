using System;

namespace TravianAnalytics.Extensions {
    public static class DateTimeExtensions {
        
        /**
         * <summary>Datetime to string extensions</summary>
         */
        public static string ToDateString(this DateTime dateTime) {
            return dateTime.ToString("dd-MM-yyyy");
        }
        
        /**
         * <summary>Datetime to string extensions</summary>
         */
        public static string ToDateTimeString(this DateTime dateTime) {
            return dateTime.ToString("dd-MM-yyyy hh:mm:ss");
        }
    }
}
