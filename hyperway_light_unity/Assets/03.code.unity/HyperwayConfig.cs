using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using static Hyperway.hyperway;

namespace Hyperway {
    using head = HeaderAttribute; using gray = DisableIfAttribute; using show = ShowIfAttribute; using name = LabelAttribute; using line = HorizontalLineAttribute;

    public class HyperwayConfig: MonoBehaviour {
        [head("Mouse")]
        [name("min drag distance (dpi)")] public float mouse_min_drag_distance_dpi = 0.05f;

        [head("Camera")]
        [name("FOV distance"   )] public float camera_fov_distance    =   7;
        [name("keyboard speed" )] public float camera_keyboard_speed  =  10;
        [name("mouse max speed")] public float camera_mouse_max_speed = 150;
        [name("mouse slowdown" )] public float camera_mouse_slowdown  = 100;

        void Awake() => update_settings();

        #if UNITY_EDITOR
        void OnValidate() { if (Application.isPlaying) update_settings(); }
        #endif

        void update_settings() {
            _mouse.min_drag_dpi_distance = mouse_min_drag_distance_dpi;
            
            _camera.fov_distance         = camera_fov_distance;
            _camera.keyboard_speed       = camera_keyboard_speed;
            _camera.mouse_drag_max_speed = camera_mouse_max_speed;
            _camera.mouse_drag_slowdown  = camera_mouse_slowdown;
        }

        [UsedImplicitly] bool playing => Application.isPlaying;
    }
}