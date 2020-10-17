using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace TravianAnalytics.Extensions {
    public static class EnumExtensions {
        public static string NameForHumans(this Enum enumValue) {
            return enumValue.GetType()
                .GetMember(enumValue.ToString())
                .First()
                .GetCustomAttribute<DisplayAttribute>()
                ?.GetName() ?? enumValue.ToString();
        }

    }
}