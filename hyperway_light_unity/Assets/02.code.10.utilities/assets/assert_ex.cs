using System.Diagnostics;
using Debug = UnityEngine.Debug;

namespace Utilities.Assets {
    public static class assert_ex {
        [Conditional("UNITY_ASSERTIONS")]
        public static void assert(bool condition) => Debug.Assert(condition);
        [Conditional("UNITY_ASSERTIONS")]
        public static void assert(bool condition, string message) => Debug.Assert(condition, message);
    }
}