using System;
using static Hyperway.entity_type_flags;
using static Hyperway.hyperway;

namespace Hyperway {
    using save = SerializableAttribute;

    public partial struct entity_type {
        public void update_transform() {
            if (flags.all(moves)) {} else return;

            var ratio = _runtime.frame_to_tick_ratio;
            for (var i = 0; i < count; i++)
                transform[i].localPosition = prev_position[i].lerp(curr_position[i], ratio).to_v3_x0y();
        }
    }
}