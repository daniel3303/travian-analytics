using System;
using System.ComponentModel.DataAnnotations;

namespace TravianAnalytics.DataAnnotations {
    public class MaxValueAttribute : ValidationAttribute {
        public int Max { get; }

        public MaxValueAttribute(int max) {
            Max = max;
        }

        public string GetErrorMessage() => $"Este valor deve ser igual ou inferior a {Max}.";

        public override bool IsValid(object value) {
            if (value == null) {
                return true;
            }

            if (value is int i) {
                return i <= Max;
            }

            if (value is float f) {
                return f <= Max;
            }

            if (value is double d) {
                return d <= Max;
            }

            throw new InvalidCastException($"{value} can not be casted to a value.");
        }

    }
}