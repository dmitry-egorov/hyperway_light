using System;

namespace Utilities.Collections {
    public static class arr_ext {
        public static void expand<t>(ref t[] arr, int count) {
            if (arr == null)
                arr = new t[count];
            else
                Array.Resize(ref arr, arr.Length + count);
        }
        public static void expand<t1, t2>(ref t1[] arr1, ref t2[] arr2, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
        }
        public static void expand<t1, t2, t3>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
        }
        public static void expand<t1, t2, t3, t4>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, ref t4[] arr4, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
            expand(ref arr4, count);
        }
        public static void expand<t1, t2, t3, t4, t5>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, ref t4[] arr4, ref t5[] arr5, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
            expand(ref arr4, count);
            expand(ref arr5, count);
        }
        public static void expand<t1, t2, t3, t4, t5, t6>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, ref t4[] arr4, ref t5[] arr5, ref t6[] arr6, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
            expand(ref arr4, count);
            expand(ref arr5, count);
            expand(ref arr6, count);
        }
        public static void expand<t1, t2, t3, t4, t5, t6, t7>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, ref t4[] arr4, ref t5[] arr5, ref t6[] arr6, ref t7[] arr7, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
            expand(ref arr4, count);
            expand(ref arr5, count);
            expand(ref arr6, count);
            expand(ref arr7, count);
        }
        public static void expand<t1, t2, t3, t4, t5, t6, t7, t8>(ref t1[] arr1, ref t2[] arr2, ref t3[] arr3, ref t4[] arr4, ref t5[] arr5, ref t6[] arr6, ref t7[] arr7, ref t8[] arr8, int count) {
            expand(ref arr1, count);
            expand(ref arr2, count);
            expand(ref arr3, count);
            expand(ref arr4, count);
            expand(ref arr5, count);
            expand(ref arr6, count);
            expand(ref arr7, count);
            expand(ref arr8, count);
        }

        public static void copy_from<t>(this t[] dst, t[] src, int count) => Array.Copy(src, dst, count);
    }
}