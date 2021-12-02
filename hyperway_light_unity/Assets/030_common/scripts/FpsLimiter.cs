using UnityEngine;
using Utilities.Runtime;
using static UnityEngine.RuntimeInitializeLoadType;

namespace Common {
    public class FpsLimiter : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => singletons.check_and_create_persistant(ref instance);
        static FpsLimiter instance;

        void Update() => Application.targetFrameRate = QualitySettings.vSyncCount <= 1 ? 60 : 30;
    }
}