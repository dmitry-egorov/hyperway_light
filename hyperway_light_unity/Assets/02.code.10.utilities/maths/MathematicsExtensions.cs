using Unity.Mathematics;
using UnityEngine;

namespace Utilities.Maths {
    public static class MathematicsExtensions {
        public static Vector3 with_y(this Vector3 v3, float y) => new Vector3(v3.x, y, v3.z);
        public static Vector3 with_z(this Vector3 v3, float z) => new Vector3(v3.x, v3.y, z);
        
        public static Vector3 xy0(this Vector2 f2) => new Vector3(f2.x, f2.y, 0);
        public static Vector3 x0y(this Vector2 f2) => new Vector3(f2.x, 0, f2.y);
        public static Vector3 x0z(this Vector3 v3) => new Vector3(v3.x, 0, v3.z);
        public static Vector2  xy(this Vector3 v3) => new Vector2(v3.x, v3.y);
        public static Vector2  xz(this Vector3 v3) => new Vector2(v3.x, v3.z);
        public static Vector2  yz(this Vector3 v3) => new Vector2(v3.y, v3.z);
        
        public static Vector3 _0y0(this Vector3 v3) => new Vector3(0, v3.y, 0);
        public static Vector3 _00z(this Vector3 v3) => new Vector3(0, 0, v3.z);

        public static float2 to_f2(this Vector2 v2) => v2;

        public static float3 x0y(this float2 f2) => new float3(f2.x, 0, f2.y);
        
        public static Quaternion rotation_to(this Vector3 v0, Vector3 v1, Vector3 fallback_axis = default) {
            // Based on Stan Melax's article in Game Programming Gems
            v0 = v0.normalized;
            v1 = v1.normalized;

            var d = Vector3.Dot(v0, v1);
            // vectors are the same
            if (d >= 1.0f)
                return Quaternion.identity;
            
            // vectors are almost opposite 
            // rotate 180 degrees about the fallback axis
            if (d < 1e-6f - 1.0f) {
                // generate an axis
                if (fallback_axis == default) {
                    fallback_axis = Vector3.Cross(new Vector3(1, 0, 0), v0);
                    if (fallback_axis == default) // pick another if colinear
                        fallback_axis = Vector3.Cross(new Vector3(0, 1, 0), v0);
                    
                    fallback_axis = fallback_axis.normalized;
                }

                return Quaternion.AngleAxis(180, fallback_axis);
            }
            
            var s = math.rsqrt((1f + d) * 2f);
            var c = Vector3.Cross(v0, v1) * s;
            return new Quaternion(c.x, c.y, c.z, 0.5f / s).normalized;
         }
    }
}