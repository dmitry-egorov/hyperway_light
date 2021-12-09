using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;
using static Lanski.Utilities.constants.consts;

namespace Hyperway {
    using save    =  SerializableAttribute;
    using name    = InspectorNameAttribute;
    
    using arr_rt  = NativeArray<res_type>;
    using arr_b2  = NativeArray<batch2_u16>;
    using arr_b   = NativeArray<fixed_batch>;
    using arr_u16 = NativeArray<ushort>;

    [save] public struct building_type_id {
        public static readonly building_type_id none  = u8_max;
        public static readonly building_type_id max_count = u8_max - 1;
        
        public byte value;

        public static implicit operator byte(building_type_id i) => i.value;
        public static implicit operator building_type_id(byte b) => new building_type_id {value = b};
    }
    
    [save] public partial struct buildings {
        public byte count;

        [scenario] public arr_b   storage_capacity_arr;
        
        [scenario] public arr_b2  production_output_arr;
        [scenario] public arr_b2  production_input_arr;
        [scenario] public arr_u16 production_ticks_arr;
    }
}