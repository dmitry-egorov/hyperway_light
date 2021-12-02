using System;
using Common.spaces;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Maths;
using static Common.spaces.offset;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;
using static UnityEngine.Time;

namespace Common {
    using no_save = NonSerializedAttribute;
    
    [after(typeof(MouseDrag))]
    public class IsometricCameraRig: MonoBehaviour {
        public float fov_distance;
        public float keyboard_speed;
        public float mouse_drag_slowdown;
        public float mouse_drag_max_speed;

        void Start() {
            _position = transform.localPosition.xz();
            _camera = GetComponentInChildren<Camera>();
        }

        void Update() {
            keep_constant_fov();
            
            move_with_keys   ();
            drag_with_mouse  ();
            apply_inertia    ();

            update_transform ();

            void keep_constant_fov() {
                _camera.fieldOfView = 2.0f * Mathf.Atan(Screen.height / Screen.dpi * 0.5f / fov_distance) * Mathf.Rad2Deg;
            }
            void move_with_keys   () {
                if (anyKey) {} else return;
                    
                var dir = zero; var move = false;
                if (keys(A, LeftArrow )) {dir  = left  + up   ; move = true; }
                if (keys(D, RightArrow)) {dir += right + down ; move = true; }
                if (keys(W, UpArrow   )) {dir += up    + right; move = true; }
                if (keys(S, DownArrow )) {dir += down  + left ; move = true; }
                    
                if (move) {} else return;

                _position += dir * (deltaTime * keyboard_speed);
                    
                static bool keys(KeyCode key1, KeyCode key2) => key(key1) || key(key2);
                static bool key (KeyCode key) => GetKey(key);
            }
            void drag_with_mouse  () {
                if (down         ()) { _drag_inertia =  zero; }
                if (drag_started ()) { _is_dragged   =  true; } // TODO: ensure the mouse was not over UI when mouse button was down
                if (drag_finished()) { _is_dragged   = false; }
                    
                if (_is_dragged) {} else return;

                var prev_ray = _camera.ScreenPointToRay(prev_pos().xy0());
                var cur_ray  = _camera.ScreenPointToRay(curr_pos().xy0());

                var plane = new Plane(Vector3.up, Vector3.zero);
                plane.intersect(prev_ray, out var prev_intersect);
                plane.intersect( cur_ray, out var  cur_intersect);

                var delta = (offset)(prev_intersect - cur_intersect).xz();
                _position += delta;
                _drag_inertia = _drag_inertia.lerp(delta / deltaTime, 0.75f);

                Vector2 curr_pos  () => MouseDrag.curr_position;
                Vector2 prev_pos  () => MouseDrag.prev_position;
                bool down         () => Mouse    .is_down;
                bool drag_started () => MouseDrag.started;
                bool drag_finished() => MouseDrag.finished;
            }
            void apply_inertia    () {
                if (is_not_dragged        ()) {} else return;
                if (drag_inertia_not_faded()) {} else { _drag_inertia = zero; return; }

                _position += _drag_inertia * deltaTime;

                var cur_speed = _drag_inertia.magnitude;
                var     speed = math.clamp(cur_speed - mouse_drag_slowdown * deltaTime, 0, mouse_drag_max_speed);
                _drag_inertia *= speed / cur_speed;
                
                bool is_not_dragged()         => !_is_dragged;
                bool drag_inertia_not_faded() => _drag_inertia.sq_magnitude > 0.0001f;
            }
            void update_transform () => transform.localPosition = _position.to_v3();
        }

        [no_save]   Camera       _camera;
        [no_save] position     _position;
        [no_save]   offset _drag_inertia;
        [no_save]     bool   _is_dragged;
    }
}