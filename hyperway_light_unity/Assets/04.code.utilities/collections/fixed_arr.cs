using System;
using System.Diagnostics;
using static Unity.Collections.LowLevel.Unsafe.UnsafeUtility;

namespace Utilities.Collections {
    public struct fixed_arr_2<t> where t : struct {
        public t item0, item1;

        public t this[byte i] {
            get {
                return i switch {
                    0 => item0
                    , 1 => item1
                    , _ => throw new IndexOutOfRangeException()
                };
            }
            set {
                switch (i) {
                    case 0: item0 = value; break;
                    case 1: item1 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
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
            get {
                return i switch {
                    0 => item0
                    , 1 => item1
                    , 2 => item2
                    , 3 => item3
                    , _ => throw new IndexOutOfRangeException()
                };
            }
            set {
                switch (i) {
                    case 0: item0 = value; break;
                    case 1: item1 = value; break;
                    case 2: item2 = value; break;
                    case 3: item3 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
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
            get {
                return i switch {
                    0 => item0
                    , 1 => item1
                    , 2 => item2
                    , 3 => item3
                    , 4 => item4
                    , 5 => item5
                    , 6 => item6
                    , 7 => item7
                    , _ => throw new IndexOutOfRangeException()
                };
            }
            set {
                switch (i) {
                    case 0: item0 = value; break;
                    case 1: item1 = value; break;
                    case 2: item2 = value; break;
                    case 3: item3 = value; break;
                    case 4: item4 = value; break;
                    case 5: item5 = value; break;
                    case 6: item6 = value; break;
                    case 7: item7 = value; break;
                    default: throw new IndexOutOfRangeException();
                }
            }
        }

        public void reset() => item0 = item1 = item2 = item3 = item4 = item5 = item6 = item7 = default;
        public void   set(t value) => item0 = item1 = item2 = item3 = item4 = item5 = item6 = item7 = value;
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