using System;
using Unity.Mathematics;

namespace Utilities.Collections {
    [Serializable] public class dirty_queue<T> {
        public T[] data;
        public int first_item_i;
        public int next_item_i;
        public int capacity_step;
        
        public int Count {
            get {
                if (first_item_i == -1)
                    return 0;
                
                var d = next_item_i - first_item_i;
                return d > 0 ? d : data.Length + d;
            }
        }

        public dirty_queue(int capacity = 4, int capacity_step = 16) {
            if (data != null && data.Length != 0) 
                return;

            capacity = math.max(2, capacity);
            capacity_step = math.max(2, capacity_step);

            this.capacity_step = capacity_step;
            data = new T[capacity];
            first_item_i = -1;
            next_item_i = 0;
        }

        public void enqueue(T item) {
            // out of capacity, resize
            if (next_item_i == first_item_i) {
                var new_data = new T[data.Length + capacity_step];
                new_data[0] = data[first_item_i]; // copy the first item separately, since we use old_i != first_item_i as a condition
                
                var new_i = 1;
                var old_i = (first_item_i + 1) % data.Length;
                while (old_i != first_item_i) {
                    new_data[new_i] = data[old_i];
                    new_i++;
                    old_i = (old_i + 1) % data.Length;
                }

                new_data[new_i] = item;
                first_item_i = 0;
                next_item_i = (new_i + 1) % new_data.Length;
                data = new_data;
            }
            else {
                data[next_item_i] = item;

                if (first_item_i == -1)
                    first_item_i = next_item_i;

                next_item_i = (next_item_i + 1) % data.Length;
            }
        }

        public bool try_peek(out T item) {
            if (first_item_i == -1) {
                item = default;
                return false;
            }

            item = data[first_item_i];
            return true;
        }

        public bool try_dequeue(out T item) {
            if (!try_peek(out item))
                return false;
            
            #if UNITY_EDITOR
            data[first_item_i] = default;
            #endif
            first_item_i = (first_item_i + 1) % data.Length;
            if (first_item_i == next_item_i) { // dequeued the last item
                first_item_i = -1;
                next_item_i = 0;
            }

            return true;
        }
    }

#if UNITY_EDITOR && false
    public static class queue_tests {
        [postprocess] static void test_queue() {
            action("test queue").act(() => {
                var queue = new dirty_queue<int>();
                
                Debug.Assert(!queue.try_peek(out _));
                Debug.Assert(!queue.try_dequeue(out _));
                
                // 2
                for (var i = 0; i < 32; i++) {
                    queue.enqueue(i);
                }

                for (var i = 0; i < 32; i++) {
                    Debug.Assert(queue.try_dequeue(out var item));
                    Debug.Assert(item == i);
                }
                
                Debug.Assert(!queue.try_dequeue(out _));
                
                // 3
                for (var i = 0; i < 32; i++) {
                    queue.enqueue(i * 2);
                    queue.enqueue(i * 2 + 1);
                    Debug.Assert(queue.try_dequeue(out var item));
                    Debug.Assert(item == i);
                }
                
                for (var i = 0; i < 32; i++) {
                    Debug.Assert(queue.try_dequeue(out var item));
                    Debug.Assert(item == (i + 32));
                }
                
                Debug.Assert(!queue.try_dequeue(out _));
            });
        }
    } 
#endif
}