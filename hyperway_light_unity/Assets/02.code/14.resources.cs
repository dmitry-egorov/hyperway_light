using System;
using Unity.Collections;
using UnityEngine;
using Utilities.Collections;
using static Hyperway.hyperway;
using static Hyperway.hyperway.res_id;
using static Lanski.Utilities.constants.consts;

namespace Hyperway {
    using save    =  SerializableAttribute;
    using name    = InspectorNameAttribute;
    
    using u8_arr     = NativeArray<byte>;
    using load_8_arr = NativeArray<fixed_arr_8<res_load<ushort>>>;

    using u8 = Byte;

    public static partial class hyperway {
        public static resources _resources;

        [save] public partial struct res_id {
            public u8 value;
            public static readonly res_id none      = u8_max;
            public static readonly res_id max_count = u8_max - 1;
        
            public static implicit operator res_id(u8 v) => new res_id { value = v };
            public static implicit operator u8(res_id e) => e.value;

            public override string ToString() => value.ToString();
        }
    
        [save] public partial struct resources {
            public string[] name_arr;
        
            public void init() {
                name_arr ??= new string[max_count];
            }
        }

        [save] public partial struct res_load<t> {
            public res_id type;
            public t      amount;
        }
    
        [save] public partial struct res_multi_8_arr {
            public u8_arr     counts;
            public load_8_arr loads;
        }
    }
}