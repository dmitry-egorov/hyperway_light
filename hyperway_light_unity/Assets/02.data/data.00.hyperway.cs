using System;

namespace Hyperway {
    using save  = SerializableAttribute;

    [save] public static partial class hyperway {
        public static   mouse _mouse;
        public static  random _random;
        public static runtime _runtime;
        public static  camera _camera;
        
        public static buildings _buildings;
        public static  entities _entities;
        public static     stats _stats;
        
        public static ui_scenario _ui_scenario;
    }
}