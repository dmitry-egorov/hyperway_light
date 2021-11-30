using System;
using Unity.Mathematics;
using UnityEngine;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using save = SerializableAttribute;

    public partial struct city {
        public partial struct archetype {
            public void remember_prev_positions() {
                if (use(prev_position, curr_position)) {} else return;
                prev_position.copy_from(curr_position, count);
            }
            
            public void apply_velocities() {
                if (use(curr_position, curr_velocity)) {} else return;
                curr_position.apply(curr_velocity, count);
            }
        }
    }
    
    [save] public partial struct velocity { public float2 vec; }
    public partial struct position { public void apply(velocity v) => vec += v.vec; }

    public static partial class position_arr_ext {
        public static void apply(this position[] positions, velocity[] velocities, int count) {
            Debug.Assert(count <= positions.Length && count <= velocities.Length);
            for (var i = 0; i < count; i++)
                positions[i].apply(velocities[i]);
        } 
    }
    public static partial class random_ext {
        public static velocity next_velocity(this ref Random r                        ) => new velocity { vec = r.NextFloat2() };
        public static velocity next_velocity(this ref Random r, float2 max            ) => new velocity { vec = r.NextFloat2(max) };
        public static velocity next_velocity(this ref Random r, float2 min, float2 max) => new velocity { vec = r.NextFloat2(min, max) };
    }
}