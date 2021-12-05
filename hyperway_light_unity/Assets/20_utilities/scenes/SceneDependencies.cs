#if UNITY_EDITOR
using UnityEditor.SceneManagement;
#endif
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Scenes {
    [ExecuteInEditMode]
    public class SceneDependencies : MonoBehaviour {
        public SceneReference[] dependencies;

        void Awake() {
            if (!Application.isPlaying)
                return;
        
            foreach (var dependency in dependencies) {
                var scene_path = dependency.ScenePath;
                if (scene_is_loaded_or_pending(scene_path)) 
                    continue;

                SceneManager.LoadScene(scene_path, LoadSceneMode.Additive);
            }
        
            Destroy(gameObject);

            static bool scene_is_loaded_or_pending(string scene_path) {
                var scene = SceneManager.GetSceneByPath(scene_path);
                if (scene.isLoaded)
                    return true;
            
                // investigate: can this be omitted in a build?
                for (var i = 0; i < SceneManager.sceneCount; i++) {
                    if (SceneManager.GetSceneAt(i) == scene) {
                        return true;
                    }
                }
            
                return false;
            }
        }

#if UNITY_EDITOR
        void Update() {
            if (Application.isPlaying) 
                return;
        
            foreach (var dependency in dependencies) {
                var scene_path = dependency.ScenePath;
                var scene = SceneManager.GetSceneByPath(scene_path);
                if (!scene.isLoaded) {
                    EditorSceneManager.OpenScene(scene_path, OpenSceneMode.Additive);
                }
            }
        }
#endif
    }
}
