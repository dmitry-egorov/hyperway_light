using System;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Collections;
using Random = Unity.Mathematics.Random;

namespace Cities {
    using save = SerializableAttribute;

    [save] public partial struct velocity {
        public float2 vec;

        public float sq_magnitude => math.lengthsq(vec);
        public float    magnitude => math.length(vec);
        
        public velocity lerp(velocity other, float ratio) => math.lerp(vec, other.vec, ratio);
        
        public static velocity zero  => default;
        public static velocity operator  +(velocity o1, velocity   o2) => o1.vec + o2.vec;
        public static velocity operator  *(velocity o , float      s ) => o.vec * s;
        public static velocity operator  /(velocity o , float      s ) => o.vec / s;
        public static implicit operator velocity( float2 f2) => new velocity { vec = f2 };
        public static implicit operator velocity(Vector2 v2) => new velocity { vec = v2 };
    }

    public partial struct position {
        public void apply(velocity v) => vec += v.vec;
        public static position operator +(position p, velocity v) => p.vec + v.vec;
    }

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