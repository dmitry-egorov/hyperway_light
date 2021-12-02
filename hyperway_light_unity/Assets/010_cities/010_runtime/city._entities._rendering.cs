using System;
using Unity.Mathematics;
using UnityEngine;

namespace Cities {
    using save = SerializableAttribute;

    public partial struct city {
        public partial struct archetype {
            public void update_transform() {
                if (use(transform, prev_position, curr_position)) {} else return;
                
                var ratio = runtime.frame_to_tick_ratio;
                for (var i = 0; i < count; i++)
                    transform[i].localPosition = prev_position[i].lerp(curr_position[i], ratio).to_v3();
            }
        }
    }
}