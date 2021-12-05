using System;
using Common;
using Lanski.Utilities.boxing;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using static Hyperway.hyperway;
using static UnityEngine.RuntimeInitializeLoadType;
using static Utilities.Runtime.persistent;

namespace Hyperway.unity {
    using no_save = NonSerializedAttribute;
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    [order(-100)]
    public class HyperwayDriver: MonoBehaviour {
        #if UNITY_EDITOR
        [show("playing")] public box<hyperway> hyperway;
        #endif
        
        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => check_or_create_singleton(ref instance);
        static HyperwayDriver instance;

        #if UNITY_EDITOR
        void Awake() {
            hyperway = _data = new box<hyperway>();
        }
        #endif

        void Update() {
            if (!initialized) {
                _hyperway.start();
                initialized = true;
            }
            
            _hyperway.update();
        }
        
        [NonSerialized] bool initialized;
        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}