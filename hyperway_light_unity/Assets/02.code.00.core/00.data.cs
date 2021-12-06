using System;
using Lanski.Utilities.boxing;
using Common.spaces;
using Lanski.Plugins.Persistance;
using UnityEngine;
using static Hyperway.entity_type.properties;
using static Lanski.Utilities.constants.consts;

namespace Hyperway {
    using save  = SerializableAttribute;
    using trans = Transform;
    using cam   = Camera;
    using rand  = Unity.Mathematics.Random;
    using bits  = FlagsAttribute;

    [save] public partial struct hyperway                {
        public static box<hyperway> _data;

        public static ref  hyperway _hyperway => ref _data.value;
        public static ref     mouse _mouse    => ref _data.value.mouse;
        public static ref    random _random   => ref _data.value.random;
        public static ref   runtime _runtime  => ref _data.value.runtime;
        public static ref    camera _camera   => ref _data.value.camera;
        public static ref  entities _entities => ref _data.value.entities;

        [aggregate] public    mouse mouse;
                            
        [aggregate] public   random random;
        [aggregate] public  runtime runtime;
        [aggregate] public   camera camera;
        
        [aggregate] public entities entities;
    }

    [save] public partial struct mouse                   {
           [config] public  float min_drag_dpi_distance;
        
        [transient] public point2 position;
        [transient] public   bool is_pressed;
        [transient] public   bool is_down;
        [transient] public   bool is_up;
        
        [transient] public   bool drag_in_progress;
        [transient] public   bool drag_started;
        [transient] public   bool drag_finished;
        [transient] public point2 drag_prev_position;
        [transient] public point2 drag_start_position;
    }
    
    [save] public partial struct random                  {
         [scenario] public uint initial_seed;
         [savefile] public rand rand;
    }
    
    [save] public partial struct runtime                 {
        [transient] public  bool paused;
        [transient] public float time_till_next_tick;
        [transient] public float frame_to_tick_ratio;
    }
    
    [save] public partial struct camera                  {
           [config] public   float fov_distance;
           [config] public   float keyboard_speed;
           [config] public   float mouse_drag_max_speed;
           [config] public   float mouse_drag_slowdown;
                           
         [savefile] public  point2 position;
         [savefile] public   float fov;
                           
        [transient] public offset2 inertia;
        [transient] public    bool is_dragged;
        [transient] public   trans transform;
        [transient] public     cam unity_cam;
    }
    
    [save] public         enum   resource_type: byte     {
        matter = 0,
        energy = 1,
        food   = 2,
        count
    }

    [save] public unsafe partial struct batch            {
        public fixed uint resources[(int)resource_type.count];
    }

    [save] public partial struct entities                {
        [aggregate] public entity_type[] types;
    }

    [save] public partial struct entity_type             {
        [permanent] public properties props;

           [config] public ushort capacity; // should be serialized in scenario?
         [savefile] public ushort count;

         // position and movement
         [savefile] public  point2[] curr_position;
         [savefile] public  point2[] prev_position;
         [savefile] public offset2[] curr_velocity;
         
         // resources
         [savefile] public   batch[] storage_capacity;
         [savefile] public   batch[] storage_amount;
         
         // rendering
         [savefile] public   trans[] transform;
         
        public enum type: byte {
            [spec(u16_max, rendered         )] scenery  = 0,
            [spec(u16_max, rendered | moves )] figure   = 1,
            [spec(   1024, rendered | stores)] building = 2,
            count
        }

        [save] [bits] public enum properties: uint {
            none       = 0,

            positioned = 1 <<  0,
            moves      = 1 <<  1 | positioned,
            stores     = 1 <<  2,

            rendered   = 1 << 16 | positioned,
        }

        void fields() {
            req(positioned, ref curr_position);
            req(rendered  , ref transform);
            req(moves     , ref prev_position , ref curr_velocity);
            req(stores    , ref storage_amount, ref storage_capacity);
        }
    }
}