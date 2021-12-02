using System;
using Game;
using Unity.Mathematics;
using UnityEngine;
using Utilities.Maths;
using Utilities.Runtime;
using static Cities.offset;
using static UnityEngine.Input;
using static UnityEngine.KeyCode;
using static UnityEngine.Time;

namespace Cities {
    using save = SerializableAttribute;
    using trans = Transform;
    
    public partial struct city {
        [save] public partial struct
        camera {
            public position  position;
            public Transform rig_transform;
            public Camera    unity_camera;

            public velocity drag_inertia;
            public     bool   is_dragged;
            
            public bool is_not_dragged         => !is_dragged;
            public bool drag_inertia_not_faded => drag_inertia.sq_magnitude > 0.0001f;

            public void init() {
                position = rig_transform.localPosition.xz().to_f2();
            }

            public void update() {
                move_with_keys    ();
                drag_by_mouse     ();
                apply_drag_inertia();

                update_transform();
            }

            void move_with_keys() {
                if (anyKey) {} else return;
                
                var dir = zero; var moving = false;
                if (key(A) || key(LeftArrow )) {dir  = left  + up   ; moving = true; }
                if (key(D) || key(RightArrow)) {dir += right + down ; moving = true; }
                if (key(W) || key(UpArrow   )) {dir += up    + right; moving = true; }
                if (key(S) || key(DownArrow )) {dir += down  + left ; moving = true; }
                
                if (moving) {} else return;

                position += dir * (deltaTime * settings.keyboard_speed);
                
                static bool key(KeyCode key) => GetKey(key);
            }

            void drag_by_mouse() {
                if (down         ()) { drag_inertia = velocity.zero; return; }
                if (drag_started ()) { is_dragged =  true; } // TODO: ensure the mouse was not over UI when mouse button was down
                if (drag_finished()) { is_dragged = false; }
                
                if (is_dragged) {} else return;

                var plane = new Plane(Vector3.up, Vector3.zero);

                var prev_ray = unity_camera.ScreenPointToRay(prev_pos().xy0());
                var cur_ray  = unity_camera.ScreenPointToRay(curr_pos().xy0());

                plane.intersect(prev_ray, out var prev_intersect);
                plane.intersect( cur_ray, out var  cur_intersect);

                var delta = (offset)(prev_intersect - cur_intersect).xz();
                position += delta;

                var vel = delta / deltaTime;
                drag_inertia = math.lerp(drag_inertia.vec, vel.vec, 0.75f);

                Vector2 curr_pos  () => MouseDrag.curr_position;
                Vector2 prev_pos  () => MouseDrag.prev_position;
                bool down         () => Mouse    .is_down;
                bool drag_started () => MouseDrag.started;
                bool drag_finished() => MouseDrag.finished;
            }
            
            void apply_drag_inertia() {
                if (is_not_dragged)         {} else return;
                if (drag_inertia_not_faded) {} else { drag_inertia = velocity.zero; return; }

                position += drag_inertia * deltaTime;

                var cur_speed = drag_inertia.magnitude;
                var     speed = math.clamp(cur_speed - settings.mouse_drag_slowdown * deltaTime, 0, settings.mouse_drag_max_speed);
                drag_inertia *= speed / cur_speed;
            }

            void update_transform() => rig_transform.localPosition = position.to_v3();

            public ref settings.camera settings => ref Game.settings.data._camera;
        }
    }
}