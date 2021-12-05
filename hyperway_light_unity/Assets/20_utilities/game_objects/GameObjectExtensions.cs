using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Utilities.Reflections.naming;

namespace Utilities.GameObjects {
    using comp = Component;
    using go = GameObject;
    
    public static class GameObjectExtensions {
        public static bool _<c>(this comp comp) where c : comp => comp.has_enabled<c>();
        public static bool _<c>(this go go) where c : comp => go.has_enabled<c>();
    }

    // can't use _ in out parameters without this separation
    public static class GameObjectExtensions2 { 
        public static bool has_disabled<c>(this comp  src) where c : comp =>  src.try_get<c>(out var comp) && !comp.is_enabled();
        public static bool has_disabled<c>(this   go   go) where c : comp =>   go.try_get<c>(out var comp) && !comp.is_enabled();
        public static bool has_enabled <c>(this comp  src) where c : comp =>  src.try_get<c>(out var comp) &&  comp.is_enabled();
        public static bool has_enabled <c>(this   go   go) where c : comp =>   go.try_get<c>(out var comp) &&  comp.is_enabled();
        public static bool has         <c>(this comp comp) where c : comp => comp.try_get<c>(out _);
        public static bool has         <c>(this   go   go) where c : comp =>   go.try_get<c>(out _);
        
        public static bool absent_or_enabled<c>(this comp comp) where c : comp => !comp.try_get<c>(out var r) || r.is_enabled();

        public static bool try_get_enabled<c>(this comp comp, out c    r) where c : comp => comp.TryGetComponent<c>(out    r) &&    r.is_enabled();
        public static bool try_get_enabled<c>(this   go   go, out c comp) where c : comp =>   go.TryGetComponent<c>(out comp) && comp.is_enabled();
        public static bool try_get        <c>(this comp comp, out c    r) where c : comp => comp.TryGetComponent<c>(out    r);
        public static bool try_get        <c>(this   go   go, out c comp) where c : comp =>   go.TryGetComponent<c>(out comp);
        public static bool try_get   <c1, c2>(this comp comp, out c1 comp1, out c2 comp2) where c1 : comp where c2: comp { comp2 = default; return comp.TryGetComponent(out comp1) && comp.TryGetComponent(out comp2); }
        public static bool try_get   <c1, c2>(this   go   go, out c1 comp1, out c2 comp2) where c1 : comp where c2: comp { comp2 = default; return   go.TryGetComponent(out comp1) &&   go.TryGetComponent(out comp2); }

        public static c require<c>(this comp comp) where c : comp => comp.gameObject.require<c>();
        public static c require<c>(this go go) where c : comp {
            var component = go.get<c>();
            if (component == null)
                Debug.LogError($"Missing required component {name_of<c>()}");
            return component;
        }

        public static c get<c>(this comp comp) where c : comp => comp.GetComponent<c>();
        public static c get<c>(this go go) where c : comp => go.GetComponent<c>();
        public static (c1, c2) get<c1, c2>(this   go go  ) where c1 : comp where c2 : comp => (  go.GetComponent<c1>(),   go.GetComponent<c2>());
        public static (c1, c2) get<c1, c2>(this comp comp) where c1 : comp where c2 : comp => (comp.GetComponent<c1>(), comp.GetComponent<c2>());
        public static (c1, c2) get<c1, c2>(this   c1 comp) where c1 : comp where c2 : comp => (comp, comp.GetComponent<c2>());

        public static c get_or_add<c>(this comp comp) where c : comp => comp.TryGetComponent<c>(out var c2) ? c2 : comp.gameObject.AddComponent<c>();
        public static c get_or_add<c>(this go go) where c : comp => go.TryGetComponent<c>(out var comp) ? comp : go.AddComponent<c>();

        public static IEnumerable<Type> get_component_types(this comp comp) => comp.gameObject.get_component_types();

        public static IEnumerable<Type> get_component_types(this go go) => 
            go.GetComponents<comp>().Select(mb => mb.GetType()).OrderBy(c => c.Name);

        public static bool is_active_and_enabled(this comp comp) => comp switch {
              MonoBehaviour m => m.isActiveAndEnabled
            , Renderer r => r.gameObject.activeInHierarchy && r.enabled
            , Collider l => l.gameObject.activeInHierarchy && l.enabled
            , _ => comp.gameObject.activeInHierarchy
        };

        public static bool is_enabled(this comp comp) =>
            comp != null &&
            comp switch {
                  MonoBehaviour m => m.enabled
                , Renderer r => r.enabled
                , Collider l => l.enabled
                , _ => true
            };
        
        public static void set_enabled(this comp comp, bool enabled) {
            var _ = comp switch {
                  MonoBehaviour m => m.enabled = enabled
                , Renderer r => r.enabled = enabled
                , Collider l => l.enabled = enabled
                , _ => throw new InvalidOperationException("Can'c set enabled")
            };
        }

#if UNITY_EDITOR
        public static void copy_components_to(this go source, go target, bool overwrite = true) {
            foreach (var comp in source.GetComponents<comp>()) {
                var type = comp.GetType();
                if (type == typeof(Transform))
                    continue;

                if (!overwrite && target.GetComponent(type) != null)
                    continue;
                
                UnityEditorInternal.ComponentUtility.CopyComponent(comp);
                UnityEditorInternal.ComponentUtility.PasteComponentAsNew(target);
            }
        }
#endif
    }
}