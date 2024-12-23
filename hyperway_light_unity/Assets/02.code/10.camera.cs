using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Maths;
using static Common.spaces.offset2;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;
using static UnityEngine.Mathf;
using static UnityEngine.Screen;
using static UnityEngine.Time;

namespace Hyperway {
    using save  = SerializableAttribute;
    using trans = Transform;
    using cam   = Camera;
    
    using key = KeyCode;

    public static partial class hyperway {
        public static camera  _camera;
    
        [save] public partial struct camera {
            [config] public float fov_distance;
            [config] public float keyboard_speed;
            [config] public float mouse_drag_max_speed;
            [config] public float mouse_drag_slowdown;
                              
            [savefile] public point2 position;
            
            public   float fov;
            public offset2 inertia;
            public    bool is_dragged;
            public   trans transform;
            public     cam unity_cam;
            
            public void start() {
                position = transform.localPosition.xz();
            }

            public void update() {
                keep_constant_fov();
                
                move_with_keys ();
                drag_with_mouse();
                apply_inertia  ();

                update_view();
            }
            
            void keep_constant_fov() { 
                fov = 2.0f * Atan(height / dpi * 0.5f / fov_distance) * Rad2Deg;
            }
            
            void move_with_keys   () {
                if (anyKey) {} else return;
                    
                var dir = zero; var move = false;
                if (keys(A, LeftArrow )) {dir  = left  + up   ; move = true; }
                if (keys(D, RightArrow)) {dir += right + down ; move = true; }
                if (keys(W, UpArrow   )) {dir += up    + right; move = true; }
                if (keys(S, DownArrow )) {dir += down  + left ; move = true; }
                    
                if (move) {} else return;

                position += dir * (deltaTime * keyboard_speed);
                    
                static bool keys(key key1, key key2) => key(key1) || key(key2);
                static bool key (key key) => GetKey(key);
            }
            
            void drag_with_mouse  () {
                if (down         ()) { inertia =  zero; }
                if (drag_started ()) { is_dragged   =  true; } // TODO: ensure the mouse was not over UI when mouse button was down
                if (drag_finished()) { is_dragged   = false; }
                    
                if (is_dragged) {} else return;

                var prev_ray = unity_cam.ScreenPointToRay(prev_pos().to_v3_xy0());
                var cur_ray  = unity_cam.ScreenPointToRay(curr_pos().to_v3_xy0());

                var plane = new Plane(Vector3.up, Vector3.zero);
                plane.intersect(prev_ray, out var prev_intersect);
                plane.intersect( cur_ray, out var  cur_intersect);

                var delta = (offset2)(prev_intersect - cur_intersect).xz();
                position += delta;
                inertia = inertia.lerp(delta / deltaTime, 0.75f);

                point2 curr_pos    () => _mouse.position;
                 bool down         () => _mouse.is_down;
                point2 prev_pos    () => _mouse.drag_prev_position;
                 bool drag_started () => _mouse.drag_started;
                 bool drag_finished() => _mouse.drag_finished;
            }
            
            void apply_inertia    () {
                if (is_not_dragged) {} else return;
                if (drag_inertia_not_faded) {} else { inertia = zero; return; }

                position += inertia * deltaTime;

                var cur_speed = inertia.magnitude;
                var     speed = math.clamp(cur_speed - mouse_drag_slowdown * deltaTime, 0, mouse_drag_max_speed);
                inertia *= speed / cur_speed;
            }
            
            void update_view      () {
                transform.localPosition = position.x0y_v3();
                unity_cam.fieldOfView = fov;
            }
            
            bool is_not_dragged         => !is_dragged;
            bool drag_inertia_not_faded => inertia.sq_magnitude > 0.0001f;
        }
    }
}