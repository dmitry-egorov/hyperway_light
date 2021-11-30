using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Utilities.Assertions {
    public static class debug_ex {
        [Conditional("UNITY_ASSERTIONS")]
        public static void else_fail(this bool condition) => Debug.Assert(condition);
        
        [Conditional("UNITY_ASSERTIONS")]
        public static void fail(this string message) => Debug.Assert(false, message);
        
        [Conditional("UNITY_ASSERTIONS")]
        public static void else_fail(this bool condition, string message) => Debug.Assert(condition, message);

        public static void log_warning(this string message) => Debug.LogWarning(message);
        public static void log_error  (this string message) => Debug.LogError  (message);
    }
}