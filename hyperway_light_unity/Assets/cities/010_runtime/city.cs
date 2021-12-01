using System;
using System.Runtime.CompilerServices;
using Utilities.Profiling;
using Utilities.Strings;

namespace Cities {
    using cn   = CallerMemberNameAttribute;
    using ln   = CallerLineNumberAttribute;
    using save = SerializableAttribute;
    
    [save] public partial struct 
    city {
        public delegate ref city data_source();
        public static data_source _data_source;

        public static ref city data => ref _data_source();

        public void init() {
            _archetypes = new archetype[1];
            _random.init();
            _camera.init();
        }

        public void update() {
            _runtime.run_sim((ref city _) => _.update_simulation   ());
            _runtime.run_vis((ref city _) => _.update_visualisation());
        }
        
        void update_simulation() {
            using var _ = profiler_ex.profile();
            
            for_each((ref archetype _) => _.remember_prev_positions());
            for_each((ref archetype _) => _.apply_velocities       ());
        }

        void update_visualisation() {
            using var _ = profiler_ex.profile();

            _camera.update();
            
            // entities
            for_each((ref archetype _) => _.update_transform());
        }

        void for_each(archetype_action action, [ln] int ln = 0) {
            using var _ = profiler_ex.profile(ln.cached_to_string());

            for (var arch_i = 0; arch_i < _archetypes.Length; arch_i++) {
                ref var archetype = ref _archetypes[arch_i];
                if (archetype.count != 0) { } else continue;
                action(ref archetype);
            }
        }

        delegate void archetype_action(ref archetype archetype);
    }
}