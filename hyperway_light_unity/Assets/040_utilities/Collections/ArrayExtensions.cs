using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Collections {
    public static class ArrayExtensions {
        public static bool contains<T>(this T[] arr, T item) {
            for (var i = 0; i < arr.Length; i++)
                if (arr[i].Equals(item))
                    return true;
            return false;
        }
        
        public static T2[] map<T1, T2>(this T1[] arr, Func<T1, T2> s) {
            var result = new T2[arr.Length];
            for (var i = 0; i < arr.Length; i++) 
                result[i] = s(arr[i]);
            return result;
        }
        
        public static void map_into<T1, T2>(this T1[] arr, Func<T1, T2> s, List<T2> list2) {
            foreach (var item1 in arr) 
                list2.Add(s(item1));
        }
        
        public static T min_by<T>(this T[] arr, Func<T, int> selector) {
            var found = arr.try_min_by(selector, out var item);
            Debug.Assert(found, "array was empty");
            return item;
        }

        public static bool try_min_by<T>(this T[] arr, Func<T, int> selector, out T item) {
            if (arr.Length == 0) {
                item = default;
                return false;
            }

            var min_item = arr[0];
            var min_value = selector(min_item);

            for (var i = 1; i < arr.Length; i++) {
                var cur_item = arr[i];
                var value = selector(cur_item);
                if (value < min_value) {
                    min_value = value;
                    min_item = cur_item;
                }
            }

            item = min_item;
            return true;
        }

        public static bool null_empty_or_any<T>(this T[] arr, Func<T, bool> predicate) => arr == null || arr.Length == 0 || arr.any(predicate);
        public static bool any<T>(this T[] arr, Func<T, bool> predicate) {
            foreach (var item in arr) {
                if (predicate(item))
                    return true;
            }

            return false;
        }
        public static bool any<T, TArg>(this T[] arr, TArg arg, Func<TArg, T, bool> predicate) {
            foreach (var item in arr) {
                if (predicate(arg, item))
                    return true;
            }

            return false;
        }
        
        public static bool all<T>(this T[] arr, Func<T, bool> predicate) {
            foreach (var item in arr) {
                if (!predicate(item))
                    return false;
            }

            return true;
        }

        public static bool try_index_of<T>(this T[] arr, T item, out int i) {
            for (i = 0; i < arr.Length; i++) {
                if (arr[i].Equals(item))
                    return true;
            }

            i = -1;
            return false;
        }

        public static int index_of<T>(this T[] arr, T item) {
            for (var i = 0; i < arr.Length; i++) {
                if (arr[i].Equals(item))
                    return i;
            }

            return -1;
        }
        
        public static int index_of<T, TArg>(this T[] arr, TArg arg, Func<TArg, T, bool> p) {
            for (var i = 0; i < arr.Length; i++) {
                var item = arr[i];
                if (p(arg, item))
                    return i;
            }

            return -1;
        }

        public static T first<T, TArg>(this T[] list, TArg arg, Func<TArg, T, bool> p) {
            for (var i = 0; i < list.Length; i++) {
                var item = list[i];
                if (p(arg, item))
                    return item;
            }
            
            throw new InvalidOperationException("Item not found");
        }

        public static bool try_first<T, TArg>(this T[] list, TArg arg, Func<TArg, T, bool> p, out T value) {
            for (var i = 0; i < list.Length; i++) {
                var item = list[i];
                if (p(arg, item)) {
                    value = item;
                    return true;
                }
            }

            value = default;
            return false;
        }
        
        public static bool try_single<T>(this T[] arr, out T value) {
            if (arr.Length != 1) {
                value = default;
                return false;
            }

            value = arr[0];
            return true;
        }

        public static int count<T>(this T[] arr, Func<T, bool> predicate) {
            var count = 0;
            for (var i = 0; i < arr.Length; i++) {
                if (predicate(arr[i])) 
                    count++;
            }

            return count;
        }

        public static bool count_greater_than<T>(this T[] arr, Func<T, bool> predicate, int min_count) {
            var count = 0;
            for (var i = 0; i < arr.Length; i++) {
                if (!predicate(arr[i])) 
                    continue;
                count++;
                if (count > min_count)
                    return true;
            }

            return false;
        }
        
        public static where_enumerator<T> non_null<T>(this T[] arr) where T: UnityEngine.Object => arr.where(x => x != null);
        public static where_enumerator<T> where<T>(this T[] arr, Func<T, bool> predicate) => new where_enumerator<T>(arr, predicate);
        
        public static pairs_iterator<T> pairs<T>(this T[] arr) => new pairs_iterator<T>(arr);

        public static T[] append_into<T>(this T[] src, ref T[] dst) => dst = (dst != null ? dst.Concat(src).ToArray() : src);

        public struct pairs_iterator<T> {
            readonly T[] arr;
            int index;

            public pairs_iterator(T[] arr) {
                this.arr = arr;
                index = -1;
            }
            
            public bool MoveNext() {
                index++;
                return index < arr.Length - 1;
            }

            public (T, T) Current => (arr[index], arr[index + 1]);
            public pairs_iterator<T> GetEnumerator() => this;
        }

        public struct where_enumerator<T> {
            readonly T[] arr;
            readonly Func<T, bool> predicate;
            int index;

            public where_enumerator(T[] arr, Func<T,bool> predicate) {
                this.arr = arr;
                this.predicate = predicate;
                index = -1;
            }

            public bool MoveNext() {
                while (true) {
                    index++;
                    if (index >= arr.Length)
                        return false;
                    if (predicate(Current))
                        return true;
                }
            }

            public T Current => arr[index];
            public where_enumerator<T> GetEnumerator() => this;
        }
    }
}