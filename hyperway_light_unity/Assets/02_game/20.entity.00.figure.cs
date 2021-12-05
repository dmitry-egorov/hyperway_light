using Unity.Mathematics;
using static Hyperway.entity_type_flags;
using static Utilities.Collections.arr_ext;

namespace Hyperway {
    public partial struct entity_type {
        public void make_figure_type(ushort entity_count) {
            capacity = count = entity_count;
            expand(ref prev_position, ref curr_position, ref curr_velocity, ref transform, count);
            flags = moves;
        }
        
        public void make_random_figures(ref Random random, float2 min_pos, float2 max_pos, float2 min_vel, float2 max_vel) {
            for (var i = 0; i < count; i++) {
                prev_position[i] =
                curr_position[i] = random.next_position(min_pos, max_pos);
                curr_velocity[i] = random.next_velocity(min_vel, max_vel);
            }
        }
    }
}