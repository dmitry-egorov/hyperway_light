using Unity.Mathematics;

namespace Utilities.Maths {
    public static class MathEx {
        public static float get_lerp_value(float speed, float dt) => 1 - math.pow(1 - speed, dt * 60);
    }
}