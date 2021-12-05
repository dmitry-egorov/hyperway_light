using Unity.Mathematics;
using static UnityEngine.Time;

namespace Hyperway {
    public partial struct runtime {
        public void run_sim<t>(ref t o, action<t> action) {
            if (!paused) { } else return;
            
            var sim_dt = fixedDeltaTime;
            var vis_dt = deltaTime;

            time_till_next_tick -= vis_dt;
            while (time_till_next_tick <= 0) {
                action(ref o);
                time_till_next_tick += sim_dt;
            }

            frame_to_tick_ratio = math.clamp(1 - time_till_next_tick / sim_dt, 0, 1);
        }

        public void run_vis<t>(ref t o, action<t> action) {
            action(ref o);
        }

        public delegate void action<t>(ref t o);
    }
}