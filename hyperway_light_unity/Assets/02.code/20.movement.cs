using Common.spaces;
using Lanski.Plugins.Persistance;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Utilities.Collections;
using static Hyperway.hyperway.entity_type_props;

namespace Hyperway {
    using burst = BurstCompileAttribute; using read = ReadOnlyAttribute; using write = WriteOnlyAttribute;
    using  p2_arr = NativeArray< point2>;
    using  o2_arr = NativeArray<offset2>;

    public static partial class hyperway {
        public partial struct entity_type {
            [savefile] public p2_arr prev_pos_arr;               // previous position
            [savefile] public o2_arr curr_vel_arr;               // current velocity

            public void movement_fields() {
                req(moves, ref prev_pos_arr, ref curr_vel_arr);
            }
        
            public void remember_prev_positions() {
                if (props.all(moves)) {} else return;
                prev_pos_arr.copy_from(curr_pos_arr, count);
            }
        
            public void apply_velocities() {
                if (props.all(moves)) {} else return;
                new apply_velocities_job { pos = curr_pos_arr, vel = curr_vel_arr }.Run(count);
            }
        
            [burst] struct apply_velocities_job: IJobParallelFor {
                public p2_arr pos;
                [read] public o2_arr vel;

                public void Execute(int i) => pos[i] += vel[i];
            }
        }
        
    }
}