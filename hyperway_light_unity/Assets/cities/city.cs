using System.Runtime.CompilerServices;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Profiling;
using Utilities.Strings;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using cn = CallerMemberNameAttribute;
    using ln = CallerLineNumberAttribute;
    
    public partial struct city {
        public static city instance;

        public void init(uint random_seed) {
            archetypes = new archetype[1];
            random     = new Random(random_seed);
        }

        public void update() {
            if (!paused) {
                var sim_dt = Time.fixedDeltaTime;
                var vis_dt = Time.deltaTime;

                time_till_next_tick -= vis_dt;
                while (time_till_next_tick <= 0) {
                    update_simulation();
                    time_till_next_tick += sim_dt;
                }

                frame_to_tick_ratio = math.clamp(1 - time_till_next_tick / sim_dt, 0, 1);
            }
            
            update_visualisation();
        }
        
        void update_simulation() {
            using var _ = profiler_ex.profile();
            
            for_each((ref archetype _) => _.remember_prev_positions());
            for_each((ref archetype _) => _.apply_velocities       ());
        }

        void update_visualisation() {
            using var _ = profiler_ex.profile();

            // visualisation
            for_each((ref archetype _) => _.update_transform());
        }

        void for_each(entity_action action, [ln] int ln = 0) {
            using var _ = profiler_ex.profile(ln.cached_to_string());
            
            var entity = new entity();
            
            ref var entity_i = ref entity.index;
            ref var   arch_i = ref entity.arch_index;

            for (arch_i = 0; arch_i < archetypes.Length; arch_i++) {
                var entity_count = archetypes[arch_i].count;
                for (entity_i = 0; entity_i < entity_count; entity_i++)
                    action(ref entity);
            }
        }

        void for_each(archetype_action action, [ln] int ln = 0) {
            using var _ = profiler_ex.profile(ln.cached_to_string());

            for (var arch_i = 0; arch_i < archetypes.Length; arch_i++) {
                ref var archetype = ref archetypes[arch_i];
                if (archetype.count != 0) { } else continue;
                action(ref archetype);
            }
        }


        delegate void entity_action   (ref entity entity);
        delegate void archetype_action(ref archetype archetype);
    }
}