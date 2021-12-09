using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Lanski.Utilities.assertions {
    public static class assert_ex {
        [Conditional("UNITY_ASSERTIONS")]
        public static void assert(this bool condition) => Debug.Assert(condition);
        [Conditional("UNITY_ASSERTIONS")]
        public static void assert(this bool condition, string message) => Debug.Assert(condition, message);
    }
}