using System;
using Utilities.Runtime;

namespace Game {
    using save = SerializableAttribute;
    
    [save] public partial struct 
    settings {
        public static provider<settings> data_provider;
        public static ref settings data => ref data_provider();
        
        public camera _camera;
        
        [save] public struct 
        camera {
            public float keyboard_speed;
            public float mouse_drag_max_speed;
            public float mouse_drag_slowdown;
        }
    }
}