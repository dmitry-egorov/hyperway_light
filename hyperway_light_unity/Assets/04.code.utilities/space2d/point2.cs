using System;
using Unity.Mathematics;
using UnityEngine;

namespace Common.spaces {
    using save = SerializableAttribute;

    [save] public partial struct 
    point2 {
        public float2 vec;

        public float distance_to(point2 other) => math.distance(vec, other.vec);
        public float distance_sq_to(point2 other) => math.distancesq(vec, other.vec);
        public point2 lerp(point2 other, float ratio) => math.lerp(vec, other.vec, ratio);
        public Vector3 x0y_v3() => new Vector3(vec.x, 0, vec.y);
        public Vector3 to_v3_xy0() => new Vector3(vec.x, vec.y, 0);

        public static implicit operator point2(float2  f2) => new point2 { vec = f2 };
        public static implicit operator point2(Vector2 f2) => new point2 { vec = f2 };

        public override string ToString() => $"({vec.x}, {vec.y})";
    }
}