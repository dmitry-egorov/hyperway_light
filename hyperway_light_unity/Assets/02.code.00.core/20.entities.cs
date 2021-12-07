using System.Runtime.CompilerServices;
using Utilities.Strings;
using static Hyperway.entity_type;
using static Hyperway.entity_type.type;
using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    using ln = CallerLineNumberAttribute;
    
    public partial struct entities {
        public ref entity_type this[type i] => ref types[(int)i];

        public void init() {
            types = new entity_type[(int)count];
            for (var i = first; i < count; i++) 
                this[i].req(i);
        }
        
        public void start() {
            for_each((ref entity_type _) => _.set_initial_transform());
        }

        public void update_simulation() {
            for_each((ref entity_type _) => _.remember_prev_positions());
            for_each((ref entity_type _) => _.apply_velocities());
        }

        public void update_visualisation() {
            for_each((ref entity_type _) => _.update_transform());
        }

        public void for_each(action action, [ln] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < types.Length; type_i++) {
                ref var type = ref types[type_i];
                if (type.count != 0) { } else continue;
                action(ref type);
            }
        }

        public void for_each<t>(ref t data, action<t> action, [ln] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < types.Length; type_i++) {
                ref var type = ref types[type_i];
                if (type.count != 0) { } else continue;
                action(ref data, ref type);
            }
        }
    }
    
    public static class execution_flags_ext {
        public static bool all(this props_ src, props_ check) => (src & check) == check;
        public static bool any(this props_ src, props_ check) => (src & check) != 0;
    }
}