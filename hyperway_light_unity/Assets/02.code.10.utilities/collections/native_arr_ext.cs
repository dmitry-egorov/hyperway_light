using Unity.Collections;

namespace Utilities.Collections {
    public static class native_arr_ext {
        public static void copy_from<t>(this NativeArray<t> dst, NativeArray<t> src, int count) where t : struct => NativeArray<t>.Copy(src, dst, count);
    }
}