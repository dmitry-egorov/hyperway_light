using System;
using Lanski.Utilities.boxing;
using Common.spaces;
using Lanski.Plugins.Persistance;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Hyperway {
    using save  = SerializableAttribute;
    using trans = Transform;
    using ucam  = Camera;

    [save] public partial struct hyperway     {
        public static readonly box<hyperway> _data = new box<hyperway>();
        
        public static ref      mouse _mouse      => ref _data.value.mouse;
        public static ref mouse_drag _mouse_drag => ref _data.value.mouse_drag;
        public static ref     random _random     => ref _data.value.random;
        public static ref    runtime _runtime    => ref _data.value.runtime;
        public static ref     camera _camera     => ref _data.value.camera;

        [aggregate] public      mouse mouse;
        [aggregate] public mouse_drag mouse_drag;
        
        [aggregate] public     random random;
        [aggregate] public    runtime runtime;
        [aggregate] public     camera camera;
        
        [aggregate] public entity_type[] entity_types;
    }

    [save] public partial struct mouse        {
        [transient] public point2 position;
    }
    
    [save] public partial struct mouse_drag   {
           [config] public  float min_drag_dpi_distance;

        [transient] public   bool in_progress;
        [transient] public   bool started;
        [transient] public   bool finished;
        [transient] public point2 prev_position;
        [transient] public point2 down_position;
    }
    
    [save] public partial struct random       {
         [scenario] public   uint initial_seed;
                          
         [savefile] public Random generator;
    }
    
    [save] public partial struct runtime      {
        [transient] public  bool paused;
        [transient] public float time_till_next_tick;
        [transient] public float frame_to_tick_ratio;
    }
    
    [save] public partial struct camera       {
           [config] public   float fov_distance;
           [config] public   float keyboard_speed;
           [config] public   float mouse_drag_max_speed;
           [config] public   float mouse_drag_slowdown;
                           
         [savefile] public  point2 position;
         [savefile] public   float fov;
                           
        [transient] public offset2 drag_inertia;
        [transient] public    bool is_dragged;
        [transient] public   trans transform;
        [transient] public    ucam unity_cam;
    }

    [save] public partial struct entity_type  {
        [permanent]  public entity_type_flags flags;
        
         [scenario] public ushort capacity;

         [savefile] public  ushort   count;
         [savefile] public  point2[] curr_position;
         [savefile] public  point2[] prev_position;
         [savefile] public offset2[] curr_velocity;
         [savefile] public   trans[] transform;
    }
    
    [Flags] public enum entity_type_flags: uint {
        none  = 0,
        moves = 1 << 0
    }
}