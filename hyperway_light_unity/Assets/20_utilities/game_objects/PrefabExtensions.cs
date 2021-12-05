#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Utilities.GameObjects {
    public static class PrefabExtensions {
        public static bool is_prefab(this Component self) {
        #if UNITY_EDITOR
            var ptype = PrefabUtility.GetPrefabAssetType(self);
            return ptype != PrefabAssetType.NotAPrefab && ptype != PrefabAssetType.MissingAsset;
        #else
            return self.gameObject.scene.name == null;
        #endif
        }
    }
}