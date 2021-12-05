using UnityEngine;
using static UnityEngine.RuntimeInitializeLoadType;
using static Utilities.Runtime.persistent;

namespace Common {
    public class FpsLimiter : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => check_or_create_singleton(ref instance);
        static FpsLimiter instance;

        void Update() => Application.targetFrameRate = QualitySettings.vSyncCount <= 1 ? 60 : 30;
    }
}