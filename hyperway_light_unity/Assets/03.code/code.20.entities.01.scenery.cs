using Lanski.Utilities.assertions;
using Unity.Mathematics;

namespace Hyperway {
    public partial struct entity_type {
        public void make_random_sceneries(ref Random random, ushort count, float2 min_pos, float2 max_pos) {
            var new_count = this.count + count;
            (new_count <= capacity).assert();
            
            for (var i = this.count; i < new_count; i++)
                curr_position_arr[i] = random.next_position(min_pos, max_pos);

            this.count = (ushort) new_count;
        }
    }
}