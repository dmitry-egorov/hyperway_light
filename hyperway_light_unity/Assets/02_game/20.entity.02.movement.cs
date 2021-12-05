using Utilities.Collections;
using static Hyperway.entity_type_flags;

namespace Hyperway {
    public partial struct entity_type {
        public void remember_prev_positions() {
            if (flags.all(moves)) {} else return;
            
            prev_position.copy_from(curr_position, count);
        }
        
        public void apply_velocities() {
            if (flags.all(moves)) {} else return;
            
            for (var i = 0; i < count; i++)
                curr_position[i] += curr_velocity[i];
        }
    }
}