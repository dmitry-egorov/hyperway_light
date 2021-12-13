using System;
using Common;
using Lanski.Utilities.assertions;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Serialization;
using Utilities.Collections;
using static Hyperway.BuildingType.type_;
using static Hyperway.hyperway;

namespace Hyperway {
    using    save =         SerializableAttribute;
    using no_save =        NonSerializedAttribute;
    using  former = FormerlySerializedAsAttribute;
    
    using out_of_range = ArgumentOutOfRangeException;
    
    using etype = entity_type_id;
    using   u16 = UInt16;

    [after(typeof(ScenarioConfig))]
    public class BuildingType : MonoBehaviour {
        [former("id")] public storage_spec_id storage_spec_id;

        public type_ type;
        [former("production_type")] 
        [ShowIf("@type == type_.producer")] public ProductionType productionType;
        [former("storage_slots"), former("storage_capacity")] 
        public ResourceSlot[] storageSlots;

        public etype entity_type => type switch {
              house     => etype.house
            , producer  => etype.producer
            , warehouse => etype.warehouse
            , _ => throw new out_of_range()
        };

        void Start() {
            (storageSlots.Length <= max_storage_slots).assert();

            _storage_specs.name_arr[storage_spec_id] = name;
            
            var slots_count = (byte)storageSlots.Length;
            _storage_specs.slots_count_arr[storage_spec_id] = slots_count;
            
            ref var slot_filters = ref _storage_specs.slots_filter_arr.@ref(storage_spec_id);
            ref var slot_caps    = ref _storage_specs.slots_cap_arr   .@ref(storage_spec_id);
            for (byte i = 0; i < slots_count; i++) {
                var slot = storageSlots[i];
                
                slot_filters.@ref(i) = slot.to_filter();
                slot_caps   .@ref(i) = slot.capacity;
            }
        }
        
        public enum type_ {
            house = 0,
            producer = 1,
            warehouse = 2
        }
    }

    public static partial class hyperway {
        [InlineProperty(LabelWidth = 1)] public partial struct storage_spec_id { }
    }
}