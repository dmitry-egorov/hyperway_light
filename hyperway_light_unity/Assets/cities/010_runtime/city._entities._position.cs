using System;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Cities {
    [Serializable] public partial struct position {
        public float2 vec;

        public static implicit operator position(float2 f2) => new position { vec = f2 };
    }
    public static partial class random_ext {
        public static position next_position(this ref Random r) => new position { vec = r.NextFloat2() };
        public static position next_position(this ref Random r, float2 max) => new position { vec = r.NextFloat2(max) };
        public static position next_position(this ref Random r, float2 min, float2 max) => new position { vec = r.NextFloat2(min, max) };
    }
}