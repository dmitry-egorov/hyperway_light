using System;
using System.Collections;
using System.Collections.Generic;

namespace unilang.common {
    public class vec<t>: IEnumerable<t> where t: struct {
        public static vec<t> empty => new vec<t>(4, 4);

        public t[] array;
        public int count;
        public int expand_by;
            
        public vec(int count, t[] arr, int expand_by = 128) {
            array = arr;
            this.count = count;
            this.expand_by = expand_by;
        }

        public vec(t[] arr, int expand_by = 128) {
            array = arr;
            count = arr.Length;
            this.expand_by = expand_by;
        }

        public vec(int capacity = 4, int expand_by = 128) {
            this.expand_by = expand_by;
            array = new t[capacity];
            count = 0;
        }

        public ArraySegment<t> as_segment() => new ArraySegment<t>(array, 0, count);
        public t this[in int index] => array[index];

        public vec<t> skip(int i) {
            var new_array = new t[array.Length];
            var new_count = count - i;
            Array.Copy(array, i, new_array, 0, new_count);
            return new vec<t>(new_count, new_array, expand_by);
        }

        public void add_front(t item) {
            var capacity = array.Length;
            if (count >= capacity) {
                var new_capacity = capacity < expand_by ? expand_by : capacity + expand_by;
                var new_array = new t[new_capacity];
                Array.Copy(array, 0, new_array, 1, capacity);
                array = new_array;
            }
            else {
                Array.Copy(array, 0, array, 1, count);
            }

            count++;
            array[0] = item;
        }

        public ref t add(t item) {
            count++;
            var capacity = array.Length;
            if (count > capacity) {
                var new_capacity = capacity < expand_by ? expand_by : capacity + expand_by;
                var new_array = new t[new_capacity];
                Array.Copy(array, new_array, capacity);
                array = new_array;
            }

            array[count - 1] = item;
            return ref array[count - 1];
        }

        public void add_range(vec<t> other) {
            var other_count = other.count;
            if (other_count == 0)
                return;

            if (array == null) {
                count = other_count;
                array = new t[count];
                Array.Copy(other.array, array, count);
                expand_by = other.expand_by;
                return;
            }
                
            var new_count = count + other_count;
            var capacity = array.Length;
            if (capacity < new_count) {
                var new_capacity = Math.Max(capacity, expand_by);
                while (new_capacity < new_count) new_capacity += expand_by;
                    
                var new_array = new t[new_capacity];
                Array.Copy(array, new_array, count);
                array = new_array;
            }
                
            Array.Copy(other.array, 0, array, count, other_count);
            count = new_count;
        }

        public bool try_remove_last() {
            if (count == 0)
                return false;
            count--;
            return true;
        }

        public bool try_remove_last(out t? item) {
            if (count == 0) {
                item = default;
                return false;
            }
            
            count--;
            item = array[count];
            return true;
        }

        public void clear() => count = 0;
            
        public override string ToString() => $"[{string.Join(", ", this)}]";

        public enumerator GetEnumerator() => new enumerator(this);

        IEnumerator<t> IEnumerable<t>.GetEnumerator() => GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public struct enumerator: IEnumerator<t> {
            readonly vec<t> arr;
            int index;
            public enumerator(vec<t> arr) {
                this.arr = arr;
                index = -1;
            }

            public bool MoveNext() {
                index++;
                return index < arr.count;
            }

            public void Reset() => index = -1;
            public t Current => arr[index];
            object IEnumerator.Current => Current;
            public void Dispose() { }
        }

        public bool Equals(vec<t> other) => this.seq_equal(other);
        public override bool Equals(object obj) => !ReferenceEquals(null, obj) && (ReferenceEquals(this, obj) || obj.GetType() == GetType() && Equals((vec<t>)obj));
        public override int GetHashCode() => array.seq_hash();
    }
}