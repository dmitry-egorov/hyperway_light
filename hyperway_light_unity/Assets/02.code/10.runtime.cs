using System;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Jobs;
using static UnityEngine.Time;

namespace Hyperway {
    using save  = SerializableAttribute;
    using jhandle = JobHandle;
    
    public static partial class hyperway {
        public static runtime _runtime;

        [save] public partial struct runtime {
            public  bool paused;
            public float time_till_next_tick;
            public float frame_to_tick_ratio;
        
            public jhandle job_handle;

            public void update(Action sim, Action vis) {
                complete_jobs();
                run_sim(sim);
                run_vis(vis);
            }
        
            public void complete_jobs() {
                job_handle.Complete();
            }
        
            public void run_sim(Action action) {
                if (!paused) { } else return;
            
                var sim_dt = fixedDeltaTime;
                var vis_dt = deltaTime;

                time_till_next_tick -= vis_dt;
                while (time_till_next_tick <= 0) {
                    action();
                    time_till_next_tick += sim_dt;
                }

                frame_to_tick_ratio = math.clamp(1 - time_till_next_tick / sim_dt, 0, 1);
            }

            public void run_vis(Action action) {
                action();
            }

            public void schedule<j>(j job, int count, int batch) where j : struct, IJobParallelFor => 
                job_handle = job.Schedule(count, batch, job_handle);

            public void schedule<j>(in j job, TransformAccessArray transforms) where j : struct, IJobParallelForTransform => 
                job_handle = job.Schedule(transforms, job_handle);

            public delegate void action<t>(ref t o);
        }
    }
}