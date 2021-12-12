using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Mathematics;

namespace Hyperway {
    using save = SerializableAttribute;
    using rnd  = Unity.Mathematics.Random;

    public static partial class hyperway {
        public static random _random;
    
        [save] public partial struct random {
            [scenario] public uint initial_seed;
            [savefile] public rnd rand;
        
            public void start() {
                rand = new rnd(initial_seed);
            }
        
            public offset2 next_velocity(                      ) => rand.next_velocity();
            public offset2 next_velocity(            float2 max) => rand.next_velocity(max);
            public offset2 next_velocity(float2 min, float2 max) => rand.next_velocity(min, max);
        
            public  point2 next_position(                      ) => rand.next_position();
            public  point2 next_position(            float2 max) => rand.next_position(max);
            public  point2 next_position(float2 min, float2 max) => rand.next_position(min, max);
        }
    }
    
    public static partial class random_ext {
        public static point2 next_position(this ref rnd r                        ) => new point2 { vec = r.NextFloat2() };
        public static point2 next_position(this ref rnd r,             float2 max) => new point2 { vec = r.NextFloat2(max) };
        public static point2 next_position(this ref rnd r, float2 min, float2 max) => new point2 { vec = r.NextFloat2(min, max) };

        public static offset2 next_velocity(this ref rnd r                        ) => new offset2 { vec = r.NextFloat2() };
        public static offset2 next_velocity(this ref rnd r,             float2 max) => new offset2 { vec = r.NextFloat2(max) };
        public static offset2 next_velocity(this ref rnd r, float2 min, float2 max) => new offset2 { vec = r.NextFloat2(min, max) };
    }
}