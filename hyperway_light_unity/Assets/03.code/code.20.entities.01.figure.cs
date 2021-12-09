using Lanski.Utilities.assertions;
using Unity.Mathematics;

namespace Hyperway {
    public partial struct entity_type {
        public void make_random_figures(ref Random random, ushort count, float2 min_pos, float2 max_pos, float2 min_vel, float2 max_vel) {
            var new_count = this.count + count;
            (new_count <= capacity).assert();

            for (var i = this.count; i < new_count; i++) { 
                prev_position_arr[i] =
                curr_position_arr[i] = random.next_position(min_pos, max_pos);
                curr_velocity_arr[i] = random.next_velocity(min_vel, max_vel);
            }
            
            this.count = (ushort) new_count;
        }
    }
}