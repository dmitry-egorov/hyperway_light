namespace Utilities.Nullable {
    public static class NullableExtensions {
        public static bool try_get<T>(this T? n, out T value) where T: struct{
            if (n.HasValue) {
                value = n.Value;
                return true;
            }

            value = default;
            return false;
        }
    }
}