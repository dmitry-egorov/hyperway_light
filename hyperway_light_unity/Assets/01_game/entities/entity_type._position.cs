using Common.spaces;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Hyperway {
    public partial struct random {
        public point2 next_position() => generator.next_position();
        public point2 next_position(float2 max) => generator.next_position(max);
        public point2 next_position(float2 min, float2 max) => generator.next_position(min, max);
    }
    
    public static partial class random_ext {
        public static point2 next_position(this ref Random r) => new point2 { vec = r.NextFloat2() };
        public static point2 next_position(this ref Random r, float2 max) => new point2 { vec = r.NextFloat2(max) };
        public static point2 next_position(this ref Random r, float2 min, float2 max) => new point2 { vec = r.NextFloat2(min, max) };
    }
}