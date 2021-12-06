using Utilities.Collections;
using static Hyperway.entity_type.properties;

namespace Hyperway {
    public partial struct entity_type {
        public void remember_prev_positions() {
            if (props.all(moves)) {} else return;
            
            prev_position.copy_from(curr_position, count);
        }
        
        public void apply_velocities() {
            if (props.all(moves)) {} else return;
            
            for (var i = 0; i < count; i++)
                curr_position[i] += curr_velocity[i];
        }
    }
}