using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Collections;
using UnityEngine;
using UnityEngine.Jobs;

namespace Hyperway {
    using static entity_type.props_;
    using static Lanski.Utilities.constants.consts;

    using save  = SerializableAttribute;
    using trans = Transform;
    using cam   = Camera;
    using bits  = FlagsAttribute;
    
    [save] public partial struct camera                  {
        [config] public float fov_distance;
        [config] public float keyboard_speed;
        [config] public float mouse_drag_max_speed;
        [config] public float mouse_drag_slowdown;
                          
        [savefile] public point2 position;
        
        public   float fov;
        public offset2 inertia;
        public    bool is_dragged;
        public   trans transform;
        public     cam unity_cam;
    }
    
    [save] public         enum   resource_type: byte     {
        matter = 0,
        energy = 1,
        water  = 2,
        food   = 3,
        
        count,
        first = 0,
    }

    [save] public partial struct batch                   {
        unsafe fixed uint amounts[(int)resource_type.count];
    }

    [save] public partial struct entities                {
        public entity_type[] types;
    }

    [save] public partial struct entity_type             {
        [permanent] public props_ props;
        [permanent] public ushort capacity;
        
         [savefile] public ushort count;

         // position and movement
         [savefile] public NativeArray< point2> curr_position;
         [savefile] public NativeArray< point2> prev_position;
         [savefile] public NativeArray<offset2> curr_velocity;
         
         // resources
         [savefile] public NativeArray<  batch> resources_capacity;
         [savefile] public NativeArray<  batch> resources_amount;
         
         // rendering
         [savefile] public TransformAccessArray transform;
         
        public enum type: byte {
            [spec(u16_max, rendered         )] scenery  = 0,
            [spec(u16_max, rendered | moves )] figure   = 1,
            [spec(   1024, rendered | stores)] building = 2,
            
            count,
            first = 0 
        }

        [save, bits] public enum props_: uint {
            none       = 0,

            positioned = 1 <<  0,
            moves      = 1 <<  1 | positioned,
            stores     = 1 <<  2,

            rendered   = 1 << 16 | positioned,
        }

        void fields() {
            req(positioned, ref curr_position);
            req(rendered  , ref transform);
            req(moves     , ref prev_position     , ref curr_velocity);
            req(stores    , ref resources_capacity, ref resources_amount);
        }
    }

    [save] public partial struct stats                   {
        public batch total_stored;
    }
}