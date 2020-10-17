using System;
using System.ComponentModel.DataAnnotations;

namespace TravianAnalytics.DataAnnotations {
    public class MinValueAttribute : ValidationAttribute {
        public int Min { get; }

        public MinValueAttribute(int min) {
            Min = min;
        }

        public string GetErrorMessage() => $"Este valor deve ser igual ou superior a {Min}.";

        public override bool IsValid(object value) {
            if (value == null) {
                return true;
            }

            if (value is int i) {
                return i >= Min;
            }

            if (value is float f) {
                return f >= Min;
            }

            if (value is double d) {
                return d >= Min;
            }

            throw new InvalidCastException($"{value} can not be casted to a value.");
        }

    }
}