using static Hyperway.hyperway;
using static UnityEngine.Screen;

namespace Hyperway {
    public partial struct mouse_drag {
        public void update() {
            started  = false;
            finished = false;
            
            if (!_mouse.is_pressed) {
                if (in_progress) { finished = true; in_progress = false; }
                return;
            }

            if (_mouse.is_down) down_position = _mouse.position;

            var min_drag_distance = dpi * min_drag_dpi_distance;
            if (!in_progress && down_position.distance_to(_mouse.position) > min_drag_distance) {
                in_progress   = true;
                started       = true;
                prev_position = down_position;
            }
        }

        public void reset() => prev_position = _mouse.position;
    }
}