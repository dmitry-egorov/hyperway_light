using System;
using Unity.Mathematics;
using UnityEngine;

namespace Cities {
    using save = SerializableAttribute;

    public partial struct city {
        [save] public partial struct 
        runtime {
            public bool  sim_paused         ;
            public float time_till_next_tick;
            public float frame_to_tick_ratio;

            public void run_sim(city_action action) {
                if (!sim_paused) {
                    var sim_dt = Time.fixedDeltaTime;
                    var vis_dt = Time.deltaTime;

                    time_till_next_tick -= vis_dt;
                    while (time_till_next_tick <= 0) {
                        action(ref data);
                        time_till_next_tick += sim_dt;
                    }

                    frame_to_tick_ratio = math.clamp(1 - time_till_next_tick / sim_dt, 0, 1);
                }
            }

            public void run_vis(city_action action) {
                action(ref data);
            }
        }

        public delegate void city_action(ref city city);
    }
}