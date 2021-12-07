using Unity.Mathematics;
using static Lanski.Utilities.assertions.assert_ex;

namespace Hyperway {
    public partial struct entity_type {
        public void make_random_figures(ref Random random, ushort count, float2 min_pos, float2 max_pos, float2 min_vel, float2 max_vel) {
            var new_count = this.count + count;
            assert(new_count <= capacity);

            for (var i = this.count; i < new_count; i++) { 
                prev_position[i] =
                curr_position[i] = random.next_position(min_pos, max_pos);
                curr_velocity[i] = random.next_velocity(min_vel, max_vel);
            }
            
            this.count = (ushort) new_count;
        }
    }
}