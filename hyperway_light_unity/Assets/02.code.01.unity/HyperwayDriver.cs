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

        void Awake() {
            _data = hyperway ??= new box<hyperway>();
            _hyperway.init();
        }

        void Update() {
            if (!initialized) {
                _data ??= hyperway;
                _hyperway.start();
                initialized = true;
            }
            
            _hyperway.update();
        }
        
        [NonSerialized] bool initialized;
        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}