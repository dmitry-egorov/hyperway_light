using System.Linq;
using System.Runtime.CompilerServices;
using unilang.common;
using Utilities.Strings;
using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    public partial struct hyperway {
        public void init() {
            random.init();
            camera.init();
            init_entities();
        }

        public void update() {
            mouse     .update();
            mouse_drag.update();
            
            runtime.run_sim(ref this, (ref hyperway _) => _.update_simulation());
            runtime.run_vis(ref this, (ref hyperway _) => {
                _.camera.update();
                _.update_visualisation();
            });

            mouse_drag.reset();
        }
        
        public static ref entity_type new_entity_type() => ref proto_entity_types.add(default);
        
        public void init_entities() {
            entity_types = proto_entity_types.ToArray();
            proto_entity_types = vec<entity_type>.empty;
        }

        public void update_simulation() {
            using var _ = profile();
            
            for_each((ref entity_type _) => _.remember_prev_positions());
            for_each((ref entity_type _) => _.apply_velocities       ());
        }

        public void update_visualisation() {
            using var _ = profile();

            // entities
            for_each((ref entity_type _) => _.update_transform());
        }

        void for_each(type_action action, [CallerLineNumber] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < entity_types.Length; type_i++) {
                ref var type = ref entity_types[type_i];
                if (type.count != 0) { } else continue;
                action(ref type);
            }
        }

        delegate void type_action(ref entity_type entity_type);
        
        static vec<entity_type> proto_entity_types = new vec<entity_type>(16, 16);
    }
}