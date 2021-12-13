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

        [former("houses_people")]
        public bool housesPeople;
        [former("storage_slots"), former("storage_capacity")] 
        public ResourceSlot[] storageSlots;
        [former("production_type")] 
        public ProductionType productionType;

        void Start() {
            (storageSlots.Length <= 8).assert();
            var slots_count = (byte)storageSlots.Length;
            _storage_specs.slots_count_arr[id] = slots_count;
            
            ref var slot_filters = ref _storage_specs.slots_filter_arr.@ref(id);
            ref var slot_caps  = ref _storage_specs.slots_cap_arr .@ref(id);
            for (byte i = 0; i < slots_count; i++) {
                var slot = storageSlots[i];
                
                slot_filters.@ref(i) = slot.to_filter();
                slot_caps   .@ref(i) = slot.capacity;
            }
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct storage_spec_id { }
    }
}