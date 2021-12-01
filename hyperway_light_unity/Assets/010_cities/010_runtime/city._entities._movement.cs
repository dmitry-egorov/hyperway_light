using System;
using Unity.Mathematics;
using Utilities.Collections;
using Random = Unity.Mathematics.Random;

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
            public velocity next_velocity(                      ) => generator.next_velocity();
            public velocity next_velocity(float2 max            ) => generator.next_velocity(max);
            public velocity next_velocity(float2 min, float2 max) => generator.next_velocity(min, max);
        }
    }

    public static partial class random {
        public static velocity next_velocity(this ref Random r                        ) => new velocity { vec = r.NextFloat2() };
        public static velocity next_velocity(this ref Random r, float2 max            ) => new velocity { vec = r.NextFloat2(max) };
        public static velocity next_velocity(this ref Random r, float2 min, float2 max) => new velocity { vec = r.NextFloat2(min, max) };
    }
}