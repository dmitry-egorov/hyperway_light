using System;
using Unity.Collections;
using UnityEngine;
using Utilities.Collections;
using static Hyperway.hyperway;
using static Hyperway.hyperway.res_id;
using static Lanski.Utilities.constants.consts;
using static Utilities.Collections.arr_ext;

namespace Hyperway {
    using save    =  SerializableAttribute;
    using name    = InspectorNameAttribute;
    
    using bit_arr    = NativeBitArray;
    using u8_arr     = NativeArray<byte>;
    using load_8_arr = NativeArray<fixed_arr_8<res_load<ushort>>>;

    using u8 = Byte;
    using u16 = UInt16;

    public static partial class hyperway {
        public static resources _resources;

        [save] public partial struct res_id {
            public u8 value;

            public string name => value == none ? "none" : _resources.name_arr[value];

            public static readonly res_id none   = u8_max;
            public static readonly int max_count = u8_count;
        
            public static implicit operator res_id(u8 v) => new res_id { value = v };
            public static implicit operator u8(res_id e) => e.value;

            public override string ToString() => name;
        }

        [save] public struct res_filter {
            public u8 value;

            public static res_filter from(res_id res_id) => new res_filter { value = res_id.value };
            public static readonly res_filter none = u8_max;
            public static readonly res_filter any  = u8_max - 1;
            public static readonly res_filter food = u8_max - 2;
            public static readonly res_filter last = food;

            public bool matches(res_id res_id) => 
                   value == any  && res_id != res_id.none
                || value == food && res_id.is_food()
                || value <  last && value == res_id;
            
            public static implicit operator res_filter(u8 v) => new res_filter { value = v };
            public static implicit operator u8(res_filter e) => e.value;
            public static implicit operator res_filter(res_id e) => e.value;

            public override string ToString() =>
                  value == none      ? "none"
                : value == any       ? "any"
                : value == food      ? "food"
                                     : "unknown";
        }
    
        [save] public partial struct resources {
            public string[] name_arr;
            public bit_arr  is_food_arr;

            public void init() {
                arr_ext.init(ref name_arr, max_count);
                is_food_arr.init(max_count);
            }
        }
        
        public static bool is_food(this res_id res_id) => _resources.is_food_arr.IsSet(res_id);

        [save] public partial struct res_load<t> {
            public res_id res;
            public t      amount;
        }
    
        [save] public partial struct res_multi_8_arr {
            public u8_arr     counts;
            public load_8_arr loads;
        }
    }
}