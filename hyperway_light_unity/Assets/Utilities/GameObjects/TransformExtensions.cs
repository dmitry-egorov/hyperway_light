using UnityEngine;

namespace Utilities.GameObjects {
    public static class TransformExtensions {
        public static float distance_to(this Transform dst, Transform src) => (dst.localPosition - src.localPosition).magnitude;

        public static void copy_from(this Transform dst, Transform src) {
            dst.SetParent(src.parent);
            dst.SetPositionAndRotation(src.localPosition, src.localRotation);
            dst.localScale = src.localScale;
        }

        public static void order_after(this Transform dst, Transform src) => dst.SetSiblingIndex(src.GetSiblingIndex() + 1);

        public static Transform create_sibling(this Transform t, string suffix) {
            var sibling_go = new GameObject($"{t.name}.{suffix}");
            var sibling_t = sibling_go.transform;
            sibling_t.copy_from(t);
            sibling_t.order_after(t);
            return sibling_t;
        }

        public static bool try_instantiate_sibling<T>(this Transform t, T prefab, string suffix, out T instance) where T: Component {
            if (!prefab.is_prefab()) {
                instance = default;
                return false;
            }
            
            instance = Object.Instantiate(prefab);
            instance.name = $"{t.name}.{suffix}";
            var bt = instance.transform;
            bt.copy_from(t);
            bt.order_after(t);

            return true;
        }

    }
}