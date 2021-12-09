using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using UnityEngine;

namespace Hyperway {
    using save  = SerializableAttribute;
    using trans = Transform;
    using cam   = Camera;

    [save] public partial struct camera {
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
}