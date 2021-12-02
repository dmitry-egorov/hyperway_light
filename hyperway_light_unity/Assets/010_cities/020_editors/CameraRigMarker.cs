using Common;
using Game.editors;
using UnityEngine;

namespace Cities.editors {
    [after(typeof(GameSettings))]
    public class CameraRigMarker : MonoBehaviour {
        public void Start() {
            var rig = gameObject.AddComponent<IsometricCameraRig>();
            ref var setting = ref Game.settings.data._camera;

            rig.fov_distance         = setting.fov_distance;
            rig.keyboard_speed       = setting.keyboard_speed;
            rig.mouse_drag_max_speed = setting.mouse_drag_max_speed;
            rig.mouse_drag_slowdown  = setting.mouse_drag_slowdown;
        }
    }
}