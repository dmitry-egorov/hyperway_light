using UnityEngine;
using Utilities.Maths;
using static Common.Mouse;
using static UnityEngine.RuntimeInitializeLoadType;
using static Utilities.Runtime.singletons;

namespace Common {
    public class MouseDrag : MonoBehaviour {
        public float min_drag_dpi_distance = 0.05f;

        public static    bool in_progress   { get; private set; }
        public static    bool started       { get; private set; }
        public static    bool finished      { get; private set; }
        public static Vector2 curr_position => position.xy();
        public static Vector2 prev_position { get; private set; }
        public static Vector2 down_position { get; private set; }

        void Update() {
            started  = false;
            finished = false;
            
            if (!is_pressed) {
                if (in_progress) { finished = true; in_progress = false; }
                return;
            }

            if (is_down) down_position = curr_position;

            var min_drag_distance = Screen.dpi * min_drag_dpi_distance;
            if (!in_progress && distance(down_position, curr_position) > min_drag_distance) {
                in_progress   = true;
                started       = true;
                prev_position = down_position;
            }
            
            static float distance(Vector2 v1, Vector2 v2) => Vector2.Distance(v1, v2);
        }

        void LateUpdate() => prev_position = curr_position;

        [RuntimeInitializeOnLoadMethod(BeforeSceneLoad)]
        static void Load() => check_and_create_persistant(ref instance);
        static MouseDrag instance;
    }
}