using UnityEngine;

namespace Common {
    [DefaultExecutionOrder(-110)]
    public class Mouse : MonoBehaviour {
        public static Vector3 position   => Input.mousePosition;
        public static    bool is_pressed => Input.GetMouseButton(0);
        public static    bool is_down    => Input.GetMouseButtonDown(0);
        public static    bool is_up      => Input.GetMouseButtonUp(0);
    }
}