using Common.spaces;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Utilities.Collections;
using static Hyperway.entity_type.props_;

namespace Hyperway {
    using burst = BurstCompileAttribute; using read = ReadOnlyAttribute; using write = WriteOnlyAttribute;
    using point_arr = NativeArray<point2>; using offset_arr = NativeArray<offset2>; 
    
    public partial struct entity_type {
        public void remember_prev_positions() {
            if (props.all(moves)) {} else return;
            prev_position.copy_from(curr_position, count);
        }
        
        public void apply_velocities() {
            if (props.all(moves)) {} else return;
            new apply_velocities_job { pos = curr_position, vel = curr_velocity }.Run(count);
        }
        
        [burst] struct apply_velocities_job: IJobParallelFor {
                   public point_arr  pos;
            [read] public offset_arr vel;

            public void Execute(int i) => pos[i] += vel[i];
        }
    }
}