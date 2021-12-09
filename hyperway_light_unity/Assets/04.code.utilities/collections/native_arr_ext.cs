using System;
using System.Diagnostics;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

namespace Utilities.Collections {
    public static class native_arr_ext {
        public static void copy_from<t>(this NativeArray<t> dst, NativeArray<t> src, int count) where t : struct => NativeArray<t>.Copy(src, dst, count);
        
        public static unsafe ref t get_ref<t>(this NativeArray<t> array, int index) where t : struct {
            array.check(index);
            return ref UnsafeUtility.ArrayElementAsRef<t>(array.GetUnsafePtr(), index);
        }
        
        [Conditional("ENABLE_UNITY_COLLECTIONS_CHECKS")]
        [Conditional("UNITY_EDITOR")]
        static void check<t>(this NativeArray<t> arr, int index) where t : struct {
            if (index < 0 || index >= arr.Length) {
                throw new IndexOutOfRangeException();
            }
        }
    }
}