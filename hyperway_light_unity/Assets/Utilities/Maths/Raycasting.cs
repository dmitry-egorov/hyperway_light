using UnityEngine;

namespace Utilities.Maths {
    public static class Raycasting {
        public static bool intersect(this Plane p, Ray r, out Vector3 intersection) {
            if (!p.Raycast(r, out var enter)) {
                intersection = default;
                return false;
            }

            intersection = r.GetPoint(enter);
            return true;
        }
    }
}