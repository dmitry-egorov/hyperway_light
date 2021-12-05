using System.Linq;
using System.Runtime.CompilerServices;
using unilang.common;
using Utilities.Strings;
using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    using ln = CallerLineNumberAttribute;
    
    public partial struct entities {
        public static ref entity_type new_entity_type() => ref proto_entity_types.add(default);
        
        public void start() {
            entity_types = proto_entity_types.ToArray();
            proto_entity_types = vec<entity_type>.empty;
        }

        public void update_simulation() {
            for_each((ref entity_type _) => _.remember_prev_positions());
            for_each((ref entity_type _) => _.apply_velocities());
        }

        public void update_visualisation() {
            for_each((ref entity_type _) => _.update_transform());
        }

        public delegate void type_action(ref entity_type entity_type);
        public void for_each(type_action action, [ln] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < entity_types.Length; type_i++) {
                ref var type = ref entity_types[type_i];
                if (type.count != 0) { } else continue;
                action(ref type);
            }
        }
        
        static vec<entity_type> proto_entity_types = new vec<entity_type>(16, 16);
    }
    
    public static class execution_flags_ext {
        public static bool all(this entity_type_flags src, entity_type_flags check) => (src & check) == check;
        public static bool any(this entity_type_flags src, entity_type_flags check) => (src & check) != 0;
    }
}