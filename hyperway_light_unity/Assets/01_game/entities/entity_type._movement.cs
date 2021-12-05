using Common.spaces;
using Unity.Mathematics;
using Utilities.Collections;
using static Hyperway.entity_type_flags;
using Random = Unity.Mathematics.Random;

namespace Hyperway {
    public partial struct entity_type {
        public void remember_prev_positions() {
            if (flags.all(moves)) {} else return;
            
            prev_position.copy_from(curr_position, count);
        }
        
        public void apply_velocities() {
            if (flags.all(moves)) {} else return;
            
            for (var i = 0; i < count; i++)
                curr_position[i] += curr_velocity[i];
        }
    }

    public partial struct random {
        public offset2 next_velocity(                      ) => generator.next_velocity();
        public offset2 next_velocity(            float2 max) => generator.next_velocity(max);
        public offset2 next_velocity(float2 min, float2 max) => generator.next_velocity(min, max);
    }

    public static partial class random_ext {
        public static offset2 next_velocity(this ref Random r                        ) => new offset2 { vec = r.NextFloat2() };
        public static offset2 next_velocity(this ref Random r, float2 max            ) => new offset2 { vec = r.NextFloat2(max) };
        public static offset2 next_velocity(this ref Random r, float2 min, float2 max) => new offset2 { vec = r.NextFloat2(min, max) };
    }
}