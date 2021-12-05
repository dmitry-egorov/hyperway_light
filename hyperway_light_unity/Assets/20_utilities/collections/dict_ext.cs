using System.Collections.Generic;

namespace Utilities.Collections {
    public static class dict_ext {
        public static TValue get_or_default<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey k) => d.TryGetValue(k, out var v) ? v : default;
    }
}