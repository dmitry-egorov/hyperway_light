using System;
using Common;
using Lanski.Utilities.assertions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Collections;
using static Hyperway.hyperway;

namespace Hyperway {
    using    save =  SerializableAttribute;
    using no_save = NonSerializedAttribute;
    using     u16 = UInt16;
    using former = FormerlySerializedAsAttribute;

    [after(typeof(ScenarioConfig))]
    public class BuildingType : MonoBehaviour {
        public storage_spec_id id;

        public bool houses_people;
        [former("storage_capacity")] 
        public ResourceLoad[] storage_slots;
        public ProductionType production_type;

        void Start() {
            (storage_slots.Length <= 8).assert();
            var slots_count = (byte)storage_slots.Length;
            _storage_specs.slots_count_arr[id] = slots_count;
            
            ref var slot_types = ref _storage_specs.slots_filter_arr.@ref(id);
            ref var slot_caps  = ref _storage_specs.slots_cap_arr .@ref(id);
            for (byte i = 0; i < slots_count; i++) {
                var cap = storage_slots[i];
                
                // todo: @nocheckin resource filters instead of types
                // implement a switch between a filter (any, food) or a specific resource
                slot_types.@ref(i) = cap.type; 
                slot_caps .@ref(i) = cap.amount;
            }
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct storage_spec_id { }
    }
}