using System.Collections.Generic;

namespace Utilities.Collections {
    public class multi_dictionary<TKey, TValue> {
        public Dictionary<TKey, List<TValue>> lists_map = new Dictionary<TKey, List<TValue>>();

        public void add(TKey k, TValue value) => get_or_create_list(k).Add(value);
        public void add_unique(TKey k, TValue value) => get_or_create_list(k).add_unique(value);
        public bool try_get(TKey key, out List<TValue> list) => lists_map.TryGetValue(key, out list);
        public List<TValue> get_or_empty(TKey key) => lists_map.TryGetValue(key, out var list) ? list : ListEx<TValue>.Empty;
        public Dictionary<TKey, List<TValue>>.Enumerator GetEnumerator() => lists_map.GetEnumerator();

        List<TValue> get_or_create_list(TKey k) {
            if (!lists_map.TryGetValue(k, out var list))
                list = lists_map[k] = new List<TValue>();
            return list;
        }
    }
}