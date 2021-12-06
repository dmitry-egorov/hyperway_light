using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    public partial struct hyperway {
        public void init() {
            entities.init();
        }
        
        public void start() {
              random.start();
              camera.start();
            entities.start();
        }

        public void update() {
            mouse.update();
            
            runtime.run_sim(ref this, (ref hyperway _) => _.update_simulation   ());
            runtime.run_vis(ref this, (ref hyperway _) => _.update_visualisation());

            mouse.reset();
        }
        
        public void update_simulation() {
            using var _ = profile();

            entities.update_simulation();
        }

        public void update_visualisation() {
            using var _ = profile();
            
            camera.update();
            
            entities.update_visualisation();
        }
    }
}