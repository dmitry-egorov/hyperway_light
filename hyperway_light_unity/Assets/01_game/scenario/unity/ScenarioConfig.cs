using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway.unity {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    public class ScenarioConfig: MonoBehaviour {
        [head("Random"      )]
        [gray("playing"     )]
        [name("initial seed")] public uint random_initial_seed = 42;
        
        void Awake() {
            update_settings();
        }

        #if UNITY_EDITOR
        void Update() => update_settings();
        #endif

        void update_settings() {
            _random.initial_seed = random_initial_seed;
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}