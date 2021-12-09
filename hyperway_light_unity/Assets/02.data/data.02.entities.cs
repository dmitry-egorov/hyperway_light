using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;
using static Hyperway.entity_type_props;

namespace Hyperway {
    using static res_type_ext;
    using static Lanski.Utilities.constants.consts;

    using save =  SerializableAttribute;
    using bits =         FlagsAttribute;
    using name = InspectorNameAttribute;
    
    using props = entity_type_props;
    using spec  = entity_type_spec;

    using arr_pt  = NativeArray<building_type_id>;
    
    using arr_u16 = NativeArray< ushort>;
    using arr_p2  = NativeArray< point2>;
    using arr_o2  = NativeArray<offset2>;

    using trans_arr = TransformAccessArray;
    
    [save] public partial struct entities {
        public entity_type[] type_arr;
    }

    public enum entity_type_id: byte {
        [spec(u16_max, rendered        )] scenery = 0,
        [spec(u16_max, rendered | moves)] figure  = 1,
        [spec(   1024, rendered | produces | consumes)] building = 2,
            
        [name(null)] count
    }

    [save, bits] public enum entity_type_props: uint {
        none       = 0,

        positioned = 1 <<  0,
        moves      = 1 <<  1 | positioned,
        stores     = 1 <<  2,
        produces   = 1 <<  3 | stores,
        consumes   = 1 <<  4 | stores,

        rendered   = 1 << 16 | positioned,
    }

    [save] public partial struct entity_type {
        [permanent] public  props props;
        [permanent] public ushort capacity;
        
        [savefile] public ushort count;

        // position and movement
        [savefile] public arr_p2 curr_position_arr;
        [savefile] public arr_p2 prev_position_arr;
        [savefile] public arr_o2 curr_velocity_arr;

        // resources
        public entity_resources resources;

        // production
        [scenario] public arr_pt    building_type_arr;
        [savefile] public arr_u16 remaining_ticks_arr;

        // rendering
        [savefile] public trans_arr transform_arr;

        void fields() {
            req(positioned, ref curr_position_arr);
            req(rendered  , ref     transform_arr);
            req(moves     , ref prev_position_arr, ref   curr_velocity_arr);
            req(produces  , ref building_type_arr, ref remaining_ticks_arr);

            foreach (var i in all_res_types) 
                req(stores, ref resources[i].stored_arr);
        }
    }
}