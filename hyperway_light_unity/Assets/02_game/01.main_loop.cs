using Lanski.Utilities.boxing;
using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    public partial struct hyperway {
        public void start() {
            random.start();
            camera.start();
            init_entities();
        }

        public void update() {
            mouse.update();
            
            runtime.run_sim(ref this, (ref hyperway _) => _.update_simulation   ());
            runtime.run_vis(ref this, (ref hyperway _) => _.update_visualisation());

            mouse.reset();
        }
        
        public void update_simulation() {
            using var _ = profile();
            
            for_each((ref entity_type _) => _.remember_prev_positions());
            for_each((ref entity_type _) => _.apply_velocities       ());
        }

        public void update_visualisation() {
            using var _ = profile();
            
            camera.update();

            // entities
            for_each((ref entity_type _) => _.update_transform());
        }
    }
}