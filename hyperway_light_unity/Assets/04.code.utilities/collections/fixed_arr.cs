using System;
using System.Diagnostics;
using static Unity.Collections.LowLevel.Unsafe.UnsafeUtility;

namespace Utilities.Collections {
    public struct fixed_arr_2<t> where t : struct {
        public t item0, item1;

        public t this[byte i] {
            get => this.@ref(i);
            set => this.@ref(i) = value;
        }

        public void reset() => item0 = item1 = default;
    }

    public static class fixed_arr_2_ext {
        public static unsafe ref t @ref<t>(this ref fixed_arr_2<t> arr, byte i) where t : struct {
            check(i);
            return ref ArrayElementAsRef<t>(AddressOf(ref arr.item0), i);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void check(byte i) {
            if (i >= 2)
                throw new IndexOutOfRangeException();
        }
    }
    
    public struct fixed_arr_4<t> where t : struct {
        public t item0, item1, item2, item3;

        public t this[byte i] {
            get => this.@ref(i);
            set => this.@ref(i) = value;
        }

        public void reset() => item0 = item1 = item2 = item3 = default;
    }

    public static class fixed_arr_4_ext {
        public static unsafe ref t @ref<t>(this ref fixed_arr_4<t> arr, byte i) where t : struct {
            check(i);
            return ref ArrayElementAsRef<t>(AddressOf(ref arr.item0), i);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void check(byte i) {
            if (i >= 4)
                throw new IndexOutOfRangeException();
        }
    }
    
    
    public struct fixed_arr_8<t> where t : struct {
        public t item0, item1, item2, item3, item4, item5, item6, item7;

        public t this[byte i] {
            get => this.@ref(i);
            set => this.@ref(i) = value;
        }

        public void reset() => item0 = item1 = item2 = item3 = item4 = item5 = item6 = item7 = default;
        public void   set(t value) => item0 = item1 = item2 = item3 = item4 = item5 = item6 = item7 = value;
        public bool   all(t value) => item0.Equals(value) && item1.Equals(value) && item2.Equals(value) && item3.Equals(value) && item4.Equals(value) && item5.Equals(value) && item6.Equals(value) && item7.Equals(value);
    }

    public static class fixed_arr_8_ext {
        public static unsafe ref t @ref<t>(this ref fixed_arr_8<t> arr, byte i) where t : struct {
            check(i);
            return ref ArrayElementAsRef<t>(AddressOf(ref arr.item0), i);
        }

        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        static void check(byte i) {
            if (i >= 8)
                throw new IndexOutOfRangeException();
        }
    }
}