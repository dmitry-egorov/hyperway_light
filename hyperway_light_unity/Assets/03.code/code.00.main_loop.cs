using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    public static partial class hyperway {
        public static void init() {
            _buildings.init();
                  _entities.init();
        }
        
        public static void start() {
                 _random.start();
                 _camera.start();
               _entities.start();

                  _stats.start();
            
            _ui_scenario.start();
        }

        public static void update() {
            _mouse.update();
            
            _runtime.complete_jobs();
            _runtime.run_sim(update_simulation);
            _runtime.run_vis(update_visualisation);
            
            _stats.update();

            _mouse.reset();
        }
        
        public static void update_simulation() {
            using var _ = profile();

            _entities.update_simulation();
        }

        public static void update_visualisation() {
            using var _ = profile();
            
            _camera.update();
            
            _entities.update_visualisation();
        }
    }
}