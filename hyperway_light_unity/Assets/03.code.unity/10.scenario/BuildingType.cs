using System;
using Common;
using Lanski.Utilities.assertions;
using Sirenix.OdinInspector;
using UnityEngine;
using Utilities.Collections;
using static Hyperway.hyperway;

namespace Hyperway {
    using    save =  SerializableAttribute;
    using no_save = NonSerializedAttribute;
    using     u16 = UInt16;

    [after(typeof(ScenarioConfig))]
    public class BuildingType : MonoBehaviour {
        public storage_spec_id id;

        public bool houses_people;
        public ResourceLoad[] storage_capacity;
        public ProductionType production_type;

        void Start() {
            (storage_capacity.Length <= 8).assert();
            var slots_count = (byte)storage_capacity.Length;
            _storage_specs.slot_count_arr[id] = slots_count;
            
            ref var slot_types = ref _storage_specs.slots_type_arr.@ref(id);
            ref var slot_caps  = ref _storage_specs.slots_cap_arr .@ref(id);
            for (byte i = 0; i < slots_count; i++) {
                var cap = storage_capacity[i];
                slot_types.@ref(i) = cap.type;
                slot_caps .@ref(i) = cap.amount;
            }
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct storage_spec_id { }
    }
}