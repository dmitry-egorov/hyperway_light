using System;
using System.Collections;
using System.Collections.Generic;

namespace Utilities.Collections {
    [Serializable] public class multi_counter_list<TKey> : IEnumerable<(TKey key, int count)> {
        public List<TKey> keys = new List<TKey>();
        public List<int> counts = new List<int>();

        public int this[TKey key] {
            get => keys.try_get_index_of(key, out var index) ? counts[index] : 0;
            set {
                if (keys.try_get_index_of(key, out var i))
                    counts[i] = value;
                else
                    new_key(key, value);
            }
        }

        public void add(TKey key, int count) {
            if (keys.try_get_index_of(key, out var i))
                counts[i] += count;
            else
                new_key(key, count);
        }

        void new_key(TKey key, int count) {
            keys.Add(key);
            counts.Add(count);
        }

        public void increment(TKey key) => add(key, 1);
        public void reset() {
            keys.Clear();
            counts.Clear();
        }

        public enumerator GetEnumerator() => new enumerator(this);
        IEnumerator<(TKey key, int count)> IEnumerable<(TKey key, int count)>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct enumerator: IEnumerator<(TKey key, int count)> {
            readonly multi_counter_list<TKey> list;
            int i;
            
            public enumerator(multi_counter_list<TKey> list) {
                this.list = list;
                i = -1;
            }
            public bool MoveNext() => (i += 1) < list.keys.Count;
            public void Reset() => i = -1;
            public (TKey key, int count) Current => (list.keys[i], list.counts[i]);
            object IEnumerator.Current => Current;
            public void Dispose() { }
        }
        
    }
}