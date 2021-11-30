using System;
using System.Collections.Generic;

namespace Utilities.Linq {
    public static class RecurseExtension {
        public static IEnumerable<T> Recurse<T>(this T item, Func<T, T> selector, bool include_self = true) where T: class {
            if (include_self)
                yield return item;

            while (true) {
                item = selector(item);
                if (item == null)
                    yield break;
                yield return item;
            }
        }
    }
}