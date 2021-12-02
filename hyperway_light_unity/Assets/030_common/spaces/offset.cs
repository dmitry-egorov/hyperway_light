using System;
using Unity.Mathematics;
using UnityEngine;
using static Unity.Mathematics.math;
using float2 = Unity.Mathematics.float2;

namespace Common.spaces {
    using save = SerializableAttribute;

    [save] public partial struct 
    offset {
        public float2 vec;

        public bool       is_zero =>      all(vec == float2.zero);
        public float sq_magnitude => lengthsq(vec);
        public float    magnitude =>   length(vec);
        
        public offset lerp(offset other, float ratio) => math.lerp(vec, other.vec, ratio);
        
        public static offset zero  => default;
        public static offset left  => (-1,  0);
        public static offset up    => ( 0,  1);
        public static offset right => ( 1,  0);
        public static offset down  => ( 0, -1);

        public static offset operator +(offset o1, offset   o2) => o1.vec + o2.vec;
        public static offset operator *(offset o , float    s ) => o.vec * s;
        public static offset operator /(offset o , float    s ) => o.vec / s;

        public static implicit operator offset((float x, float y) t) => new float2(t.x, t.y);
        public static implicit operator offset( float2 f2) => new offset { vec = f2 };
        public static implicit operator offset(Vector2 v2) => new offset { vec = v2 };

        public static position operator +(position p, offset o) => p.vec + o.vec;
    }
}