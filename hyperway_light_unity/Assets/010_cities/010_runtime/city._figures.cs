using Unity.Mathematics;
using static Utilities.Collections.arr_ext;

namespace Cities {
    public partial struct city {
        public partial struct archetype {
            public void make_figure_archetype(ushort entity_count) {
                capacity = count = entity_count;
                expand(ref prev_position, ref curr_position, ref curr_velocity, ref transform, count);
            }
            
            public void make_random_figures(ref Random random, float2 min_pos, float2 max_pos, float2 min_vel, float2 max_vel) {
                if  (use(prev_position, curr_position, curr_velocity)) { } else return;
                for (var i = 0; i < count; i++) {
                    prev_position[i] =
                    curr_position[i] = random.next_position(min_pos, max_pos);
                    curr_velocity[i] = random.next_velocity(min_vel, max_vel);
                }
            }
        }
    }
}