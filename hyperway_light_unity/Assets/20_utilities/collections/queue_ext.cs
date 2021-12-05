using System.Collections.Generic;

namespace Utilities.Collections {
    public static class queue_ext {
        public static bool TryDequeue<T>(this Queue<T> queue, out T item) {
            if (queue.Count == 0) {
                item = default;
                return false;
            }

            item = queue.Dequeue();
            return true;
        }
    }
}