using UnityEngine;

namespace Utilities.Runtime {
    public class fps_limiter : MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void Load() => try_instantiate();
        
        static void try_instantiate() {
            if (instance != null)
                return;
            
            instance = FindObjectOfType<fps_limiter>();
            if (instance == null) {
                instance = new GameObject().AddComponent<fps_limiter>();
                instance.name = "[fps_limiter]";
                DontDestroyOnLoad(instance);
            }
        }

        void Update() => Application.targetFrameRate = QualitySettings.vSyncCount <= 1 ? 60 : 30;
        
        static fps_limiter instance;
    }
}