using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

namespace Utilities.Scenes {
    public static class SceneExtensions {
        public static IEnumerable<T> GetComponentsInChildren<T>(this Scene scene, bool include_inactive) => 
            scene.GetRootGameObjects().SelectMany(x => x.GetComponentsInChildren<T>(include_inactive));
    }
}