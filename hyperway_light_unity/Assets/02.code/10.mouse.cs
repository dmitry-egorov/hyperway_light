using System;
using Common.spaces;
using Lanski.Plugins.Persistance;
using UnityEngine;
using Utilities.Maths;
using static UnityEngine.Screen;

namespace Hyperway {
    using save  = SerializableAttribute;

    public static partial class hyperway {
        public static mouse  _mouse;
    
        [save] public partial struct mouse {
            [config] public  float min_drag_dpi_distance;
            
            public point2 position;
            public   bool is_pressed;
            public   bool is_down;
            public   bool is_up;
            
            public   bool drag_in_progress;
            public   bool drag_started;
            public   bool drag_finished;
            public point2 drag_prev_position;
            public point2 drag_start_position;
            
            public void update() {
                update_position_and_buttons();
                update_drag();
            }

            void update_position_and_buttons() {
                position   = Input.mousePosition.xy();
                is_pressed = Input.GetMouseButton    (0);
                is_down    = Input.GetMouseButtonDown(0);
                is_up      = Input.GetMouseButtonUp  (0);
            }

            void update_drag() {
                drag_started = false;
                drag_finished = false;

                if (!is_pressed) {
                    if (drag_in_progress) {
                        drag_finished = true;
                        drag_in_progress = false;
                    }

                    return;
                }

                if (is_down) drag_start_position = position;

                var min_drag_distance = dpi * min_drag_dpi_distance;
                if (!drag_in_progress && drag_start_position.distance_to(_mouse.position) > min_drag_distance) {
                    drag_in_progress = true;
                    drag_started = true;
                    drag_prev_position = drag_start_position;
                }
            }

            public void reset() => drag_prev_position = _mouse.position;
        }
    }
}