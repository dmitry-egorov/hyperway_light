using UnityEngine;
using Utilities.Maths;

namespace Hyperway {
    public partial struct mouse {
        public bool is_pressed => Input.GetMouseButton    (0);
        public bool is_down    => Input.GetMouseButtonDown(0);
        public bool is_up      => Input.GetMouseButtonUp  (0);

        public void update() {
            position = Input.mousePosition.xy();
        }
    }
}