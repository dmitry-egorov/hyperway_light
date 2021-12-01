using System;
using Unity.Mathematics;
using Random = Unity.Mathematics.Random;

namespace Cities {
    [Serializable] public partial struct position {
        public float2 vec;

        public static implicit operator position(float2 f2) => new position { vec = f2 };
        public static position operator +(position p, offset o) => p.vec + o.vec;
    }
    
    [Serializable] public partial struct offset {
        public float2 vec;
        public static offset zero  => default;
        public static offset left  => (-1,  0);
        public static offset up    => ( 0,  1);
        public static offset right => ( 1,  0);
        public static offset down  => ( 0, -1);

        public static bool   operator ==(offset o1, offset   o2) => math.all(o1.vec == o2.vec);
        public static bool   operator !=(offset o1, offset   o2) => !(o1 == o2);
        public static offset operator  +(offset o1, offset   o2) => o1.vec + o2.vec;
        public static offset operator  *(offset o , float    s ) => o.vec * s;

        public static implicit operator offset((float x, float y) t) => new float2(t.x, t.y);
        public static implicit operator offset(float2 f2) => new offset { vec = f2 };
    }

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