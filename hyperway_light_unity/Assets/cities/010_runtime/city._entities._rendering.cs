using System;
using Unity.Mathematics;
using UnityEngine;

namespace Cities {
    using save = SerializableAttribute;

    public partial struct city {
        public partial struct archetype {
            public void update_transform() {
                if (use(transform, prev_position, curr_position)) {} else return;
                
                var ratio = frame_to_tick_ratio;
                for (var i = 0; i < count; i++)
                    transform[i].localPosition = prev_position[i].lerp(curr_position[i], ratio).to_vec3();
            }
        }
    }

    public partial struct position {
        public Vector3 to_vec3() => new Vector3(vec.x, 0, vec.y);
        public position lerp(position other, float ratio) => math.lerp(vec, other.vec, ratio);
    }
}