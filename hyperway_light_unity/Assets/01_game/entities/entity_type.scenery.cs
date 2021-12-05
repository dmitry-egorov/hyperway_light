using Unity.Mathematics;
using static Hyperway.entity_type_flags;
using static Utilities.Collections.arr_ext;

namespace Hyperway {
    public partial struct entity_type {
        public void make_scenery_type(ushort entity_count) {
            capacity = count = entity_count;
            expand(ref curr_position, ref transform, count);
            flags = none;
        }
        
        public void make_random_sceneries(ref Random random, float2 min_pos, float2 max_pos) {
            for (var i = 0; i < count; i++) {
                     transform[i].localPosition =
                (curr_position[i] = random.next_position(min_pos, max_pos)).to_v3_x0y();
            }
        }
    }
}