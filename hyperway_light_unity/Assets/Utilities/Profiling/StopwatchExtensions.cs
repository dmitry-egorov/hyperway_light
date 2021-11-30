using System.Diagnostics;

namespace Utilities.Profiling {
    public static class StopwatchExtensions {
        public static long Mark(this Stopwatch sw) {
            var ms = sw.ElapsedMilliseconds;
            sw.Restart();
            return ms;
        }
    }
}