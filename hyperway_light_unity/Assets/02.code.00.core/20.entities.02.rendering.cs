using System;
using static Hyperway.entity_type.properties;
using static Hyperway.hyperway;

namespace Hyperway {
    using save = SerializableAttribute;

    public partial struct entity_type {
        public void set_initial_transform() {
            if (props.all(rendered | positioned)) {} else return;

            for (var i = 0; i < count; i++)
                transform[i].localPosition = curr_position[i].to_v3_x0y();
        }

        public void update_transform() {
            if (props.all(rendered | moves)) {} else return;

            var ratio = _runtime.frame_to_tick_ratio;
            for (var i = 0; i < count; i++)
                transform[i].localPosition = prev_position[i].lerp(curr_position[i], ratio).to_v3_x0y();
        }
    }
}