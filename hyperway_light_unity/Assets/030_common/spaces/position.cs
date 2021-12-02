using System;
using Unity.Mathematics;
using UnityEngine;

namespace Common.spaces {
    using save = SerializableAttribute;

    [save] public partial struct 
    position {
        public float2 vec;

        public static implicit operator position(float2  f2) => new position { vec = f2 };
        public static implicit operator position(Vector2 f2) => new position { vec = f2 };
        
        public Vector3 to_v3() => new Vector3(vec.x, 0, vec.y);
        public position lerp(position other, float ratio) => math.lerp(vec, other.vec, ratio);
    }
}