using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Scenario.editors {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    public class ScenarioSettings: MonoBehaviour {
        [head("Random"      )]
        [gray("playing"     )]
        [name("initial seed")] public uint random_initial_seed = 42;
        
        [line(height: 1     )]
        [show("playing"     )]
        [name("data"        )] public settings settings;

        void Awake() {
            settings.data_provider = () => ref settings;
            update_settings();
        }

        #if UNITY_EDITOR
        void Update() => update_settings();
        #endif

        void update_settings() {
            settings._random.initial_seed = random_initial_seed;
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}