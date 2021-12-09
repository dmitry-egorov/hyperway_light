using System.Runtime.CompilerServices;
using Utilities.Strings;
using static Hyperway.entity_type_id;
using static Hyperway.entity_type;
using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    using props = entity_type_props;
    using ln = CallerLineNumberAttribute;
    
    public partial struct entities {
        public ref entity_type this[entity_type_id i] => ref type_arr[(int)i];

        public void init() {
            type_arr = new entity_type[(int)count];
            for (entity_type_id i = 0; i < count; i++) {
                this[i].resources = new entity_resources((int)res_type.count);
                this[i].fields(i);
            }
        }
        
        public void start() {
            for_each((ref entity_type _) => _.set_initial_transform());
        }

        public void update_simulation() {
            for_each((ref entity_type _) => _.produce());
            
            for_each((ref entity_type _) => _.remember_prev_positions());
            for_each((ref entity_type _) => _.apply_velocities());
        }

        public void update_visualisation() {
            for_each((ref entity_type _) => _.update_transform());
        }

        public void for_each(action action, [ln] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < type_arr.Length; type_i++) {
                ref var type = ref type_arr[type_i];
                if (type.count != 0) { } else continue;
                action(ref type);
            }
        }

        public void for_each<t>(ref t data, action<t> action, [ln] int ln = 0) {
            using var _ = profile(ln.cached_to_string());

            for (var type_i = 0; type_i < type_arr.Length; type_i++) {
                ref var type = ref type_arr[type_i];
                if (type.count != 0) { } else continue;
                action(ref data, ref type);
            }
        }
    }
    
    public static class execution_flags_ext {
        public static bool all(this props src, props check) => (src & check) == check;
        public static bool any(this props src, props check) => (src & check) != 0;
    }
}