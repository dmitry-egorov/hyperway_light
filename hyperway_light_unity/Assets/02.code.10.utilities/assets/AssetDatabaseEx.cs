#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using Object = UnityEngine.Object;

namespace Utilities.Assets {
    public static class AssetDatabaseEx {
        public static IEnumerable<T> FindAssetsByType<T>() where T : Object {
            var guids = AssetDatabase.FindAssets($"t:{typeof(T)}");
            foreach (var t in guids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
                if (asset != null)
                    yield return asset;
            }
        }
        
        public static IEnumerable<Object> FindAssetsByType(Type type) {
            var guids = AssetDatabase.FindAssets($"t:{type}");
            foreach (var t in guids) {
                var assetPath = AssetDatabase.GUIDToAssetPath(t);
                var asset = AssetDatabase.LoadAssetAtPath(assetPath, type);
                if (asset != null)
                    yield return asset;
            }
        }
        
        public static Object FindSingletonAssetByType(Type type) => FindAssetsByType(type).Single();
    }
}
#endif