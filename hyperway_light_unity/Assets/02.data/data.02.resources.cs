using System;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;

namespace Hyperway {
    using save    =  SerializableAttribute;
    using name    = InspectorNameAttribute;
    using arr_u32 = NativeArray<uint>;
    using arr_u16 = NativeArray<ushort>;

    [save] public partial struct entity_resources {
        public entities_resource_data[] data;
    }

    [save] public partial struct entities_resource_data {
        [savefile] public arr_u32 stored_arr;
    }
    
    [save] public enum res_type: byte {
        matter = 0,
        energy = 1,
         water = 2,
          food = 3,
        
        [name(null)] count
    }

    [save] public partial struct batch_u16 {
        public res_type type;
        public   ushort amount;
    }

    [save] public partial struct batch2_u16 {
        public batch_u16 b0;
        public batch_u16 b1;
    }

    [save] public partial struct fixed_batch {
        unsafe fixed uint amounts[(int)res_type.count];
    }
}