using UnityEngine;
using Utilities.Maths;
using static Hyperway.hyperway;
using static UnityEngine.Screen;

namespace Hyperway {
    public partial struct mouse {
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