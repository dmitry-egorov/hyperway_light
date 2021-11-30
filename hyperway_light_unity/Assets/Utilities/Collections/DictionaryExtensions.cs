using System.Collections.Generic;

namespace Utilities.Collections {
    public static class DictionaryExtensions {
        public static TValue GetOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> d, TKey k) => d.TryGetValue(k, out var v) ? v : default;
    }
}