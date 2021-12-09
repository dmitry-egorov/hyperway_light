namespace Utilities.Tuples {
    public static class TupleExtensions {
        public static (t1, t2) with<t1, t2>(this t1 v1, t2 v2) => (v1, v2);
        public static (t1, t2, t3) with<t1, t2, t3>(this t1 v1, t2 v2, t3 v3) => (v1, v2, v3);
        public static (t1, t2, t3) with<t1, t2, t3>(this t1 v1, (t2, t3) v23) => (v1, v23.Item1, v23.Item2);
        public static (t1, t2, t3) with<t1, t2, t3>(this (t1, t2) v12, t3 v3) => (v12.Item1, v12.Item2, v3);
    }
}