using static Utilities.Profiling.profiler_ex;

namespace Hyperway {
    public static partial class hyperway {
        public static void init() {
            _resources    .init();
            _prod_specs   .init();
            _storage_specs.init();
            _entities     .init();
            _stats        .init();
        }
        
        public static void start() {
            _random     .start();
            _camera     .start();
            _hunger     .start();
            _entities   .start();

            _stats      .start();
            
            _ui_scenario.start();
        }

        public static void update() {
              _mouse.update();
            _runtime.update(update_simulation, update_visualisation);
              _stats.update();
              _mouse.reset();
        }
        
        public static void update_simulation() {
            using var _ = profile();

              _hunger.update();
            _entities.update_simulation();
        }

        public static void update_visualisation() {
            using var _ = profile();
            
              _camera.update();
            _entities.update_visualisation();
        }
    }
}