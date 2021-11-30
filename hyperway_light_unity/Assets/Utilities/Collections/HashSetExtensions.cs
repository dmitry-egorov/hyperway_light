using System.Collections.Generic;

namespace Utilities.Collections {
    public static class HashSetExtensions {
        public static HashSet<T> to_hash_set<T>(this IEnumerable<T> e) {
            var hs = new HashSet<T>();
            foreach (var item in e) 
                hs.try_add(item);

            return hs;
        }
        
        public static bool try_add<T>(this HashSet<T> set, T item) {
            if (set.Contains(item))
                return false;

            set.Add(item);
            return true;
        }
    }
}