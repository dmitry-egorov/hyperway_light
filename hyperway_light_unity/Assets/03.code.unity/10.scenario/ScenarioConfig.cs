using System;
using System.Collections.Generic;
using Common;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using Utilities.Collections;
using static Hyperway.hyperway;

namespace Hyperway {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    [after(typeof(HyperwayConfig))]
    public class ScenarioConfig: MonoBehaviour {
        [head("Random")]
        [gray("playing"), name("initial seed")] public uint random_initial_seed = 42;
        
        [head("UI"    )]
        [gray("playing"), name("displayed resources")] public Resource[] ui_displayed_resources;
        
        [head("Hunger")]
        [gray("playing"), name("Max level")] public byte hunger_max_level;
        [gray("playing"), name("Cooldown ")] public byte hunger_cooldown;
        
        void Awake() {
            update_settings();
            init_resource_types();
            init_production_types();
            init_building_types();
        }

        #if UNITY_EDITOR
        void OnValidate() { if (Application.isPlaying) update_settings(); }
        #endif

        void update_settings() {
            _random.initial_seed     = random_initial_seed;
            _hunger.max_level = hunger_max_level;
            _hunger.cooldown  = hunger_cooldown;
        }

        static void init_resource_types() {
            var type_editors = FindObjectsOfType<Resource>();

            var count = type_editors.Length;
            if (count > res_id.max_count)
                throw new ApplicationException($"Maximum number of resource types ({res_id.max_count}) exceeded");
            
            var set = new HashSet<byte>();
            for (byte i = 0; i < count; i++)
                if (!set.try_add(type_editors[i].id))
                    throw new ApplicationException($"Id of {type_editors[i].name} is not unique");
        }

        static void init_production_types() {
            ref var types = ref _prod_specs;

            var type_editors = FindObjectsOfType<ProductionType>();

            var count = type_editors.Length;
            if (count > prod_spec_id.max_count)
                throw new ApplicationException($"Maximum number of production types ({prod_spec_id.max_count}) exceeded");

            types.count = (byte)count;

            var set = new HashSet<byte>();
            for (byte i = 0; i < count; i++)
                if (!set.try_add(type_editors[i].id))
                    throw new ApplicationException($"Id of {type_editors[i].name} is not unique");
        }

        static void init_building_types() {
            ref var types = ref _storage_specs;

            var type_editors = FindObjectsOfType<BuildingType>();

            var count = type_editors.Length;
            if (count > storage_spec_id.max_count)
                throw new ApplicationException($"Maximum number of building types ({storage_spec_id.max_count}) exceeded");

            types.count = (byte)count;

            var set = new HashSet<byte>();
            for (byte i = 0; i < count; i++)
                if (!set.try_add(type_editors[i].id))
                    throw new ApplicationException($"Id of {type_editors[i].name} is not unique");
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}