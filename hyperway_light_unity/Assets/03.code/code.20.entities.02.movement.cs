using Common.spaces;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Utilities.Collections;
using static Hyperway.entity_type_props;

namespace Hyperway {
    using burst = BurstCompileAttribute; using read = ReadOnlyAttribute; using write = WriteOnlyAttribute;
    using arr_p2 = NativeArray<point2>; using arr_o2 = NativeArray<offset2>; 
    
    public partial struct entity_type {
        public void remember_prev_positions() {
            if (props.all(moves)) {} else return;
            prev_position_arr.copy_from(curr_position_arr, count);
        }
        
        public void apply_velocities() {
            if (props.all(moves)) {} else return;
            new apply_velocities_job { pos = curr_position_arr, vel = curr_velocity_arr }.Run(count);
        }
        
        [burst] struct apply_velocities_job: IJobParallelFor {
                   public arr_p2 pos;
            [read] public arr_o2 vel;

            public void Execute(int i) => pos[i] += vel[i];
        }
    }
}