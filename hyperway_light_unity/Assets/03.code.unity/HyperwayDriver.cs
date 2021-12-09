using System;
using Common;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using static UnityEngine.RuntimeInitializeLoadType;
using static Utilities.Runtime.persistent;

namespace Hyperway.unity {
    using no_save = NonSerializedAttribute;
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    [order(-100)]
    public class HyperwayDriver: MonoBehaviour {
        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => check_or_create_singleton(ref instance);
        static HyperwayDriver instance;

        void Awake() {
            hyperway.init();
        }

        void Update() {
            if (!initialized) {
                hyperway.start();
                initialized = true;
            }
            
            hyperway.update();
        }
        
        [no_save] bool initialized;
        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}