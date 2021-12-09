using System.Collections.Generic;
using System.Linq;

namespace unilang.common {
    public static class enumerable_ext {
        public static bool seq_equal<t>(this IEnumerable<t> e1, IEnumerable<t> e2) => e1.SequenceEqual(e2);
        
        public static int seq_hash<t>(this IEnumerable<t> es) { unchecked {
            var hash = 19;
            foreach (var e in es) hash = hash * 31 + e?.GetHashCode() ?? 0;
            return hash;
        }}
    }
}