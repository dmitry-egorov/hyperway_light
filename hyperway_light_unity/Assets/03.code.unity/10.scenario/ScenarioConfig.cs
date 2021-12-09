using System;
using Common;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway.unity {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    [after(typeof(HyperwayConfig))]
    public class ScenarioConfig: MonoBehaviour {
        [head("Random"      )]
        [gray("playing"     )]
        [name("initial seed")] public uint random_initial_seed = 42;
        
        void Awake() {
            update_settings();
            init_building_types();
        }

        #if UNITY_EDITOR
        void OnValidate() { if (Application.isPlaying) update_settings(); }
#endif

        void update_settings() {
            _random.initial_seed = random_initial_seed;
        }

        static void init_building_types() {
            ref var types = ref _buildings;

            var type_editors = FindObjectsOfType<BuildingType>();

            var count = type_editors.Length;
            if (count > byte.MaxValue)
                throw new ApplicationException($"Maximum number of building types ({byte.MaxValue}) exceeded");

            types.count = (byte)count;

            for (byte i = 0; i < count; i++) type_editors[i].id = i;
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}