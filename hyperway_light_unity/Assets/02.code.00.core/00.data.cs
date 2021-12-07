using System;
using Lanski.Utilities.boxing;

namespace Hyperway {
    using save  = SerializableAttribute;

    [save] public partial struct hyperway                {
        public static box<hyperway> _data;

        public static ref hyperway _hyperway => ref _data.value;
        public static ref    mouse _mouse    => ref _data.value.mouse;
        public static ref   random _random   => ref _data.value.random;
        public static ref  runtime _runtime  => ref _data.value.runtime;

        public static ref   camera _camera   => ref _data.value.camera;
        public static ref entities _entities => ref _data.value.entities;
        public static ref    stats _stats    => ref _data.value.stats;

        public static ref ui_scenario _ui_scenario => ref _data.value.ui_scenario;

        public    mouse mouse;

        public   random random;
        public  runtime runtime;
        public   camera camera;

        public entities entities;
        public    stats stats;

        public ui_scenario ui_scenario;
    }

}