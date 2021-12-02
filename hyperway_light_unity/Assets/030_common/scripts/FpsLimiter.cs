using UnityEngine;
using static UnityEngine.RuntimeInitializeLoadType;

namespace Utilities.Runtime {
    public class FpsLimiter : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => singletons.check_and_create_persistant_instance(ref instance);
        static FpsLimiter instance;

        void Update() => Application.targetFrameRate = QualitySettings.vSyncCount <= 1 ? 60 : 30;
    }
}