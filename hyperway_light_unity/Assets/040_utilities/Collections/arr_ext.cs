using System;

namespace Utilities.Collections {
    public static class arr_ext {
        public static void create_or_expand<t>(ref t[] arr, int count) {
            if (arr == null)
                arr = new t[count];
            else
                Array.Resize(ref arr, arr.Length + count);
        }

        public static void copy_from<t>(this t[] dst, t[] src, int count) => Array.Copy(src, dst, count);
    }
}