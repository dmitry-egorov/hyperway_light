using System;
using Unity.Mathematics;
using Utilities.Collections;

namespace Cities {
    using save = SerializableAttribute;
    
    [save] public partial struct velocity { public float2 vec; }
    public partial struct position { public void apply(velocity v) => vec += v.vec; }

    public partial struct city {
        public partial struct archetype {
            public void remember_prev_positions() {
                if (use(prev_position, curr_position)) {} else return;
                prev_position.copy_from(curr_position, count);
            }
            
            public void apply_velocities() {
                if (use(curr_position, curr_velocity)) {} else return;
                for (var i = 0; i < count; i++)
                    curr_position[i].apply(curr_velocity[i]);
            }
        }

        public partial struct random {
            public velocity next_velocity(                      ) => new velocity { vec = generator.NextFloat2() };
            public velocity next_velocity(float2 max            ) => new velocity { vec = generator.NextFloat2(max) };
            public velocity next_velocity(float2 min, float2 max) => new velocity { vec = generator.NextFloat2(min, max) };
        }
    }
}