namespace Utilities.Flow {
    public static class nullcheck {
        public static bool not_null(object o1, object o2) => o1 != null && o2 != null;
        public static bool not_null(object o1, object o2, object o3) => o1 != null && o2 != null && o3 != null;
    }
}