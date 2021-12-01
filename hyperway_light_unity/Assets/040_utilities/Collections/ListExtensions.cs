using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Collections {
    public static class ListExtensions {
        public static void reverse_into<T>(this List<T> src, List<T> dst) {
            dst.Capacity = src.Capacity;
            for (var i = src.Count - 1; i >= 0; i--) 
                dst.Add(src[i]);
        }

        public static bool try_remove_first<T>(this List<T> list, out T item) {
            if (list.Count == 0) {
                item = default;
                return false;
            }
            item = list[0];
            list.RemoveAt(0);
            return true;
        }

        public static bool try_remove_first<T>(this List<T> list, Func<T, bool> predicate) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (predicate(item)) {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static bool try_remove_first<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> predicate) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (predicate(arg, item)) {
                    list.RemoveAt(i);
                    return true;
                }
            }

            return false;
        }

        public static T remove_last<T>(this List<T> list) {
            var item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return item;
        }

        public static bool try_remove_last<T>(this List<T> list, out T item) {
            if (list.Count == 0) {
                item = default;
                return false;
            }
            item = list[list.Count - 1];
            list.RemoveAt(list.Count - 1);
            return true;
        }

        public static void swap_back_at<T>(this List<T> list, int i) {
            if (i >= list.Count)
                throw new IndexOutOfRangeException();

            var last_index = list.Count - 1;
            list[i] = list[last_index];
            list.RemoveAt(last_index);
        }

        public static bool is_empty<T>(this List<T> list) => list.Count == 0;
        public static bool try_first<T>(this List<T> list, out T value) {
            if (list.is_empty()) {
                value = default;
                return false;
            }

            value = list[0];
            return true;
        }

        public static T first<T>(this List<T> list) => list[0];
        public static T first<T>(this List<T> list, Func<T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(item))
                    return item;
            }

            throw new InvalidOperationException("None of the items in the list satisfy the condition");
        }
        public static T first<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(arg, item))
                    return item;
            }

            throw new InvalidOperationException("None of the items in the list satisfy the condition");
        }
        
        public static bool try_first<T>(this List<T> list, Func<T, bool> p, out T value) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(item)) {
                    value = item;
                    return true;
                }
            }

            value = default;
            return false;
        }
        
        public static bool try_first<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> p, out T value) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(arg, item)) {
                    value = item;
                    return true;
                }
            }

            value = default;
            return false;
        }
        
        public static T first_or_default<T>(this List<T> list, Func<T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(item))
                    return item;
            }

            return default;
        }
        
        public static T first_or_default<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(arg, item))
                    return item;
            }

            return default;
        }
        
        public static bool try_single<T>(this List<T> list, out T value) {
            if (list.Count != 1) {
                value = default;
                return false;
            }

            value = list[0];
            return true;
        }

        public static int index_of<T>(this List<T> list, T item) => list.IndexOf(item);
        public static int index_of<T>(this List<T> list, Func<T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(item))
                    return i;
            }

            return -1;
        }
        
        public static int index_of<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> p) {
            for (var i = 0; i < list.Count; i++) {
                var item = list[i];
                if (p(arg, item))
                    return i;
            }

            return -1;
        }
        
        public static bool try_get_index_of<T>(this List<T> list, T item, out int i) {
            i = 0;
            for (; i < list.Count; i++) {
                var e = list[i];
                if (e.Equals(item))
                    return true;
            }

            return false;
        }
        
        public static bool try_get_index_of<T>(this List<T> list, Func<T, bool> p, out int i) {
            i = 0;
            for (; i < list.Count; i++) {
                var item = list[i];
                if (p(item))
                    return true;
            }

            return false;
        }
        
        public static bool try_get_index_of<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> p, out int i) {
            i = 0;
            for (; i < list.Count; i++) {
                var item = list[i];
                if (p(arg, item))
                    return true;
            }

            return false;
        }
        
        public static T last<T>(this List<T> list) => list[list.Count - 1];

        public static where_enumerator<T> non_null<T>(this List<T> list) where T: UnityEngine.Object => list.where(x => x != null);

        public static where_enumerator<T> where<T>(this List<T> list, Func<T, bool> predicate) {
            return new where_enumerator<T>(list, predicate);
        }

        public static bool empty_or_any<T>(this List<T> list, Func<T, bool> predicate) => list.Count == 0 || list.any(predicate);
        public static bool any<T>(this List<T> list, Func<T, bool> predicate) {
            foreach (var item in list) {
                if (predicate(item))
                    return true;
            }

            return false;
        }
        public static bool any<T, TArg>(this List<T> list, TArg arg, Func<TArg, T, bool> predicate) {
            foreach (var item in list) {
                if (predicate(arg, item))
                    return true;
            }

            return false;
        }
        
        public static bool all<T>(this List<T> list, Func<T, bool> predicate) {
            foreach (var item in list) {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static void select_into<T1, T2>(this List<T1> list1, Func<T1, T2> s, List<T2> list2) {
            foreach (var item1 in list1)
                list2.Add(s(item1));
        }

        public static bool try_min_by<T>(this List<T> list, Func<T, int> selector, out T item) {
            if (list.Count == 0) {
                item = default;
                return false;
            }

            var min_item = list[0];
            var min_value = selector(min_item);

            for (var i = 1; i < list.Count; i++) {
                var cur_item = list[i];
                var value = selector(cur_item);
                if (value < min_value) {
                    min_value = value;
                    min_item = cur_item;
                }
            }

            item = min_item;
            return true;
        }

        public static T min_by<T>(this List<T> list, Func<T, int> selector) {
            var found = list.try_min_by(selector, out var item);
            Debug.Assert(found, "list was empty");
            return item;
        }

        public static bool add_unique<T>(this List<T> list, T item) {
            if (list.Contains(item)) 
                return false;
            
            list.Add(item);
            return true;
        }
        
        public static pairs_iterator<T> pairs<T>(this List<T> arr) => new pairs_iterator<T>(arr);

        public struct pairs_iterator<T> {
            readonly List<T> arr;
            int index;

            public pairs_iterator(List<T> arr) {
                this.arr = arr;
                index = -1;
            }
            
            public bool MoveNext() {
                index++;
                return index < arr.Count - 1;
            }

            public (T, T) Current => (arr[index], arr[index + 1]);
            public pairs_iterator<T> GetEnumerator() => this;
        }

        public struct where_enumerator<T> {
            readonly List<T> list;
            readonly Func<T, bool> predicate;
            int index;

            public where_enumerator(List<T> list, Func<T,bool> predicate) {
                this.list = list;
                this.predicate = predicate;
                index = -1;
            }

            public bool MoveNext() {
                while (true) {
                    index++;
                    if (index >= list.Count)
                        return false;
                    if (predicate(Current))
                        return true;
                }
            }

            public T Current => list[index];
            public where_enumerator<T> GetEnumerator() => this;
        }
    }
}