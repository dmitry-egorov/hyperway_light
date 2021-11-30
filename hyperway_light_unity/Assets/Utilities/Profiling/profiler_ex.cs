using System;
using System.Runtime.CompilerServices;
using static UnityEngine.Profiling.Profiler;

namespace Utilities.Profiling {
    public static class profiler_ex {
        public static profiling_context profile([CallerMemberName]string name = null) {
            BeginSample(name);
            return new profiling_context();
        }
        public struct profiling_context: IDisposable {
            public void Dispose() => EndSample();
        }
    }
}