using System;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using float2 = Unity.Mathematics.float2;

namespace Common.spaces {
    using save = SerializableAttribute;

    [save] public partial struct 
    offset2 {
        public float2 vec;

        public bool       is_zero =>      all(vec == float2.zero);
        public float sq_magnitude => lengthsq(vec);
        public float    magnitude =>   length(vec);
        
        public offset2 lerp(offset2 other, float ratio) => math.lerp(vec, other.vec, ratio);
        
        public static offset2 zero  => default;
        public static offset2 left  => (-1,  0);
        public static offset2 up    => ( 0,  1);
        public static offset2 right => ( 1,  0);
        public static offset2 down  => ( 0, -1);

        public static offset2 operator +(offset2 o1, offset2  o2) => o1.vec + o2.vec;
        public static offset2 operator *(offset2 o , float    s ) => o.vec * s;
        public static offset2 operator /(offset2 o , float    s ) => o.vec / s;

        public static implicit operator offset2((float x, float y) t) => new float2(t.x, t.y);
        public static implicit operator offset2( float2 f2) => new offset2 { vec = f2 };
        public static implicit operator offset2(Vector2 v2) => new offset2 { vec = v2 };
    }
    
    public partial struct point2 {
        public static point2 operator +(point2 p, offset2 o) => p.vec + o.vec;
    }
}