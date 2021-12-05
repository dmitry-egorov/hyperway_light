using System;
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

    public class HyperwayDriver: MonoBehaviour {
        [show("playing")]public box<hyperway> data;

        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => check_or_create_singleton(ref instance);
        static HyperwayDriver instance;

        void Awake() {
            data = _data;
        }

        void Update() {
            if (!initialized) {
                _data.value.init();
                initialized = true;
            }
            
            _data.value.update();
        }
        
        [NonSerialized] bool initialized;
        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}