using System.Collections.Generic;

namespace Utilities.Strings {
    public static class number_strings_cache {
        public static string cached_to_string(this int i) => line_numbers.TryGetValue(i, out var str) ? str : line_numbers[i] = i.ToString();
        static Dictionary<int, string> line_numbers = new Dictionary<int, string>();
    }
}