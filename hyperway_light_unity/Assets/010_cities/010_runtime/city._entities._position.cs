using Common.spaces;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Cities {
    public partial struct city {
        public partial struct random {
            public position next_position() => generator.next_position();
            public position next_position(float2 max) => generator.next_position(max);
            public position next_position(float2 min, float2 max) => generator.next_position(min, max);
        }
    }
    
    public static partial class random_ext {
        public static position next_position(this ref Random r) => new position { vec = r.NextFloat2() };
        public static position next_position(this ref Random r, float2 max) => new position { vec = r.NextFloat2(max) };
        public static position next_position(this ref Random r, float2 min, float2 max) => new position { vec = r.NextFloat2(min, max) };
    }
}