using System.Text.RegularExpressions;

namespace TravianAnalytics.Extensions {
    public static class StringExtensions {
        public static string Truncate(this string value, int maxLength) {
            if (string.IsNullOrEmpty(value)) return value;
            return value.Length <= maxLength ? value : value.Substring(0, maxLength);
        }
        
        public static bool IsValidPortugueseVat(this string vat) {
            if (vat.Length != 9) return false;
            var added = 
                (int) char.GetNumericValue(vat[7]) * 2 + 
                (int) char.GetNumericValue(vat[6]) * 3 + 
                (int) char.GetNumericValue(vat[5]) * 4 + 
                (int) char.GetNumericValue(vat[4]) * 5 + 
                (int) char.GetNumericValue(vat[3]) * 6 + 
                (int) char.GetNumericValue(vat[2]) * 7 +
                (int) char.GetNumericValue(vat[1]) * 8 + 
                (int) char.GetNumericValue(vat[0]) * 9;
            var mod = added % 11;
            var control = 11 - mod;
            if (mod == 0 || mod == 1) {
                control = 0;
            }

            return (int)char.GetNumericValue(vat[8]) == control;
        }
        
        public static bool IsValidPortugueseZipcode(this string zipCode) {
            if (zipCode.Length != 8) return false;
            var zipCodeRegex = new Regex(@"^[0-9]{4}-[0-9]{3}$");
            if (zipCodeRegex.IsMatch(zipCode)) {
                return true;
            }

            return false;
        }

    }
}